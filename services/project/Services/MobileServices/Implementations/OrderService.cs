using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Models.Db.DbCart;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.Payments;
using Models.Db.RestaurantStop;
using Models.Db.Schedule;
using Models.DTOs.External;
using Models.DTOs.Misc;
using Models.DTOs.Orders;
using Models.Misc;
using Newtonsoft.Json;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;
using Services.MobileServices.Abstractions;
using Services.Shared.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRequestAccountIdService _requestAccountIdService;
        private readonly IOnlinePaymentRepository _onlinePaymentRepository;
        private readonly IOrderPostProcessingQueue _orderPostProcessingQueue;
        private readonly IPaymentService _paymentService;

        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ICartItemRepository cartItemRepository, IOrderRepository orderRepository, IMapper mapper, IPriceGroupRepository priceGroupRepository, IMenuItemRepository menuItemRepository, ICityRepository cityRepository, IRestaurantRepository restaurantRepository, IRequestAccountIdService requestAccountIdService, IPaymentService paymentService, IOnlinePaymentRepository onlinePaymentRepository, IOrderPostProcessingQueue orderPostProcessingQueue, ILogger<OrderService> logger)
        {
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _priceGroupRepository = priceGroupRepository;
            _menuItemRepository = menuItemRepository;
            _cityRepository = cityRepository;
            _restaurantRepository = restaurantRepository;
            _requestAccountIdService = requestAccountIdService;
            _paymentService = paymentService;
            _onlinePaymentRepository = onlinePaymentRepository;
            _orderPostProcessingQueue = orderPostProcessingQueue;
            _logger = logger;
        }

        public async Task<CreatedDto> CreateFromCart(MobileCreateOrderFromCartDto mobileCreateOrderFromCartDto)
        {
            using var _ = _logger.BeginScope("m/Order/CreateFromCartV1: {data}", JsonConvert.SerializeObject(mobileCreateOrderFromCartDto));

            if (await _orderRepository.GetOne(o => o.UniqueId == mobileCreateOrderFromCartDto.UniqueId) != null)
            {
                _logger.LogError("UniqueId Constraint Violation {unique_id}", mobileCreateOrderFromCartDto.UniqueId);
                await TelegramAPI.Send($"Order.CreateFromCart: Нарушена идентичность запроса (UniqueId): {mobileCreateOrderFromCartDto.UniqueId}");
                throw new AkianaException("Нарушена идентичность запроса (UniqueId)");
            }

            var restaurant = await _restaurantRepository.GetByIdNonTracking(
                mobileCreateOrderFromCartDto.RestaurantId,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.PickupStops.OrderByDescending(s => s.Id)
                    .Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id)
                    .Take(1)
            );

            if ((restaurant.SupportedPaymentTypes & mobileCreateOrderFromCartDto.PaymentType.ToDb()) == PaymentTypeFlags.Undefined)
            {
                _logger.LogError(
                    "Unsupported Payment Type {restaurant_id} {requested} {available}",
                    restaurant.Id,
                    mobileCreateOrderFromCartDto.PaymentType.ToString(),
                    restaurant.SupportedPaymentTypes.ToBits()
                        .ToReadableString()
                );
                await TelegramAPI.Send(
                    "Unsupported Payment Type:\n" +
                    $"RestaurantId: {restaurant.Id}\n" +
                    $"Request PaymentType: {mobileCreateOrderFromCartDto.PaymentType.ToString()}\n" +
                    $"Supported PaymentTypes: {restaurant.SupportedPaymentTypes.ToBits().ToReadableString()}"
                );

                throw new AkianaException("Ресторан не поддерживает данный тип оплаты.");
            }

            var gmtOffsetByRestaurant = await _cityRepository.GetGmtOffsetByRestaurant(mobileCreateOrderFromCartDto.RestaurantId);

            EnsureThatRestaurantIsNotStoppedAtTheMomentOfMakingAnOrder(mobileCreateOrderFromCartDto.PickupType, restaurant);

            var localTimeByServer = DateTime.Now.AddHours(gmtOffsetByRestaurant);

            var localTimeOfDay = localTimeByServer.TimeOfDay;

            EnsureThatLocalTimeIsCorrectWhenPerformingAnOrder(mobileCreateOrderFromCartDto.PickupType, restaurant, localTimeOfDay);

            EnsureThatIfDelayedAwaitedIsAfterCurrentTime(
                mobileCreateOrderFromCartDto.DelayType,
                mobileCreateOrderFromCartDto.AwaitedAtDateTime,
                localTimeByServer,
                localTimeOfDay
            );

            EnsureDeliveryAddressIdIsNotPresentForPickup(mobileCreateOrderFromCartDto.PickupType, mobileCreateOrderFromCartDto.DeliveryAddressId);

            var (orderItems, menuItemsIds, cartItems) = await CloneUsersCartIntoOrderOrderItems(mobileCreateOrderFromCartDto.ClientAccountId);

            // Construct an order
            var order = _mapper.Map<Order>(mobileCreateOrderFromCartDto);

            order.CreatedAtDateTime = DateTime.Now;

            CalculateAwaitedAtIfImmediate(order, localTimeByServer);

            order.OrderItems = orderItems;

            // Calculate useless order.Title in "CountInADay - ItemsCount" format
            var countTodayOrders = await _orderRepository.Count(o => o.CreatedAtDateTime > DateTime.Now.AddDays(-1));

            order.Title = $"{countTodayOrders + 1} - {orderItems.Count}";

            await CalculateOrderItemPrices(
                mobileCreateOrderFromCartDto.RestaurantId,
                mobileCreateOrderFromCartDto.ClientAccountId,
                menuItemsIds,
                orderItems
            );

            var requestAccountId = _requestAccountIdService.Id;

            order.CreatorWorkerAccountId = requestAccountId;
            order.UniqueId = mobileCreateOrderFromCartDto.UniqueId;

            await _orderRepository.Add(order);

            // Clear user's cart if it's not online payment
            await _cartItemRepository.RemoveMany(cartItems);

            await _orderPostProcessingQueue.Enqueue(requestAccountId, order.Id);

            return order.Id;
        }

        public async Task<OrderCreationResultDto> CreateFromCartV2(MobileCreateOrderFromCartV2Dto mobileCreateOrderFromCartDto)
        {
            using var _ = _logger.BeginScope("m/Order/CreateFromCartV2: {data}", JsonConvert.SerializeObject(mobileCreateOrderFromCartDto));

            if (await _orderRepository.GetOne(o => o.UniqueId == mobileCreateOrderFromCartDto.UniqueId) != null)
            {
                _logger.LogError("UniqueId Constraint Violation {unique_id}", mobileCreateOrderFromCartDto.UniqueId);
                await TelegramAPI.Send($"Order.CreateFromCartV2: Нарушена идентичность запроса (UniqueId): {mobileCreateOrderFromCartDto.UniqueId}");
                throw new AkianaException("Нарушена идентичность запроса (UniqueId)");
            }

            var restaurant = await _restaurantRepository.GetByIdNonTracking(
                mobileCreateOrderFromCartDto.RestaurantId,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.PickupStops.OrderByDescending(s => s.Id)
                    .Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id)
                    .Take(1)
            );

            if ((restaurant.SupportedPaymentTypes & mobileCreateOrderFromCartDto.PaymentType) == PaymentTypeFlags.Undefined)
            {
                _logger.LogError(
                    "Unsupported Payment Type {restaurant_id} {requested} {available}",
                    restaurant.Id,
                    mobileCreateOrderFromCartDto.PaymentType.ToString(),
                    restaurant.SupportedPaymentTypes.ToBits()
                        .ToReadableString()
                );
                await TelegramAPI.Send(
                    "Unsupported Payment Type:\n" +
                    $"RestaurantId: {restaurant.Id}\n" +
                    $"Request PaymentType: {mobileCreateOrderFromCartDto.PaymentType.ToString()}\n" +
                    $"Supported PaymentTypes: {restaurant.SupportedPaymentTypes.ToBits().ToReadableString()}"
                );

                throw new AkianaException("Ресторан не поддерживает данный тип оплаты.");
            }

            var gmtOffsetByRestaurant = await _cityRepository.GetGmtOffsetByRestaurant(mobileCreateOrderFromCartDto.RestaurantId);

            EnsureThatRestaurantIsNotStoppedAtTheMomentOfMakingAnOrder(mobileCreateOrderFromCartDto.PickupType, restaurant);

            var localTimeByServer = DateTime.Now.AddHours(gmtOffsetByRestaurant);

            var localTimeOfDay = localTimeByServer.TimeOfDay;

            EnsureThatLocalTimeIsCorrectWhenPerformingAnOrder(mobileCreateOrderFromCartDto.PickupType, restaurant, localTimeOfDay);

            EnsureThatIfDelayedAwaitedIsAfterCurrentTime(
                mobileCreateOrderFromCartDto.DelayType,
                mobileCreateOrderFromCartDto.AwaitedAtDateTime,
                localTimeByServer,
                localTimeOfDay
            );

            EnsureDeliveryAddressIdIsNotPresentForPickup(mobileCreateOrderFromCartDto.PickupType, mobileCreateOrderFromCartDto.DeliveryAddressId);

            var (orderItems, menuItemsIds, cartItems) = await CloneUsersCartIntoOrderOrderItems(mobileCreateOrderFromCartDto.ClientAccountId);

            // Construct an order
            var order = _mapper.Map<Order>(mobileCreateOrderFromCartDto);

            order.CreatedAtDateTime = DateTime.Now;

            CalculateAwaitedAtIfImmediate(order, localTimeByServer);

            order.OrderItems = orderItems;

            // Calculate useless order.Title in "CountInADay - ItemsCount" format
            var countTodayOrders = await _orderRepository.Count(o => o.CreatedAtDateTime > DateTime.Now.AddDays(-1));

            order.Title = $"{countTodayOrders + 1} - {orderItems.Count}";

            await CalculateOrderItemPrices(
                mobileCreateOrderFromCartDto.RestaurantId,
                mobileCreateOrderFromCartDto.ClientAccountId,
                menuItemsIds,
                orderItems
            );

            var requestAccountId = _requestAccountIdService.Id;

            order.CreatorWorkerAccountId = requestAccountId;
            order.UniqueId = mobileCreateOrderFromCartDto.UniqueId;

            await _orderRepository.Add(order);

            if (mobileCreateOrderFromCartDto.PaymentType == PaymentTypeFlags.Online)
            {
                SberbankRegisterResultDto sberbankRegisterResultDto = null;
                try
                {
                    if (string.IsNullOrEmpty(restaurant.SberbankUsername) || string.IsNullOrEmpty(restaurant.SberbankPassword))
                    {
                        _logger.LogError(
                            "No Username Or Password Specified for Restaurant {restaurant_id} {login} {password}",
                            restaurant.Id,
                            restaurant.SberbankUsername,
                            restaurant.SberbankPassword
                        );
                        await PayError.NoUsernameOrPassword(restaurant.Id)
                            .Throw(mobileCreateOrderFromCartDto.ClientAccountId, null, requestAccountId);
                    }

                    var sum = orderItems.Sum(i => i.PurchasePrice);
                    sberbankRegisterResultDto = await _paymentService.CreatePayment(
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword,
                        order.Id,
                        sum,
                        $"Заказ в мобильном приложении: {order.Id}",
                        mobileCreateOrderFromCartDto.ClientAccountId
                    );

                    if (sberbankRegisterResultDto.ErrorCode is not null)
                    {
                        _logger.LogError(
                            "Sberbank ErrorCode was not null {restaurant_id} {login} {password} {response}",
                            restaurant.Id,
                            restaurant.SberbankUsername,
                            restaurant.SberbankPassword,
                            sberbankRegisterResultDto
                        );
                        await PayError.SberbankRegisterErrorCode(sberbankRegisterResultDto.ErrorCode)
                            .Throw(mobileCreateOrderFromCartDto.ClientAccountId, null, requestAccountId);
                    }

                    _logger.LogInformation(
                        "Sberbank Executed Payment Request {restaurant_id} {login} {password} {order_id} {external_id} {client_id} {sum}",
                        restaurant.Id,
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword,
                        order.Id,
                        sberbankRegisterResultDto.OrderId,
                        mobileCreateOrderFromCartDto.ClientAccountId,
                        sum
                    );

                    await TelegramAPI.Send(
                        $"**SBERBANK Executed Payment Request**\n" +
                        $"OrderId: {order.Id}\n" +
                        $"ExternalId: {sberbankRegisterResultDto.OrderId}\n" +
                        $"ClientId: {mobileCreateOrderFromCartDto.ClientAccountId}\n" +
                        $"Sum: {sum}"
                    );
                }
                catch (AkianaException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Sberbank CreatePayment Network Error {restaurant_id} {login} {password}",
                        restaurant.Id,
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword
                    );
                    await PayError.CreatePaymentNetworkError
                        .Throw(mobileCreateOrderFromCartDto.ClientAccountId, null, requestAccountId);
                }

                var onlinePayment = new OnlinePayment()
                {
                    OrderId = order.Id,
                    ConfirmationSource = ConfirmationSource.Unconfirmed,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedAt = DateTime.Now,
                    ConfirmedAt = null,
                    IssuerId = requestAccountId,
                    ExternalId = sberbankRegisterResultDto.OrderId
                };

                await _onlinePaymentRepository.Add(onlinePayment);

                _logger.LogInformation(
                    "Created Payment Order {order_id}, {restaurant_id} {login} {password}",
                    order.Id,
                    restaurant.Id,
                    restaurant.SberbankUsername,
                    restaurant.SberbankPassword
                );

                return new OrderCreationResultDto()
                {
                    OrderId = order.Id,
                    PaymentUrl = sberbankRegisterResultDto.FormUrl
                };
            }

            // Clear user's cart if it's not online payment
            await _cartItemRepository.RemoveMany(cartItems);

            await _orderPostProcessingQueue.Enqueue(requestAccountId, order.Id);

            return new OrderCreationResultDto()
            {
                OrderId = order.Id
            };
        }

        private async Task CalculateOrderItemPrices(long restaurantId, long clientAccountId, List<long> menuItemsIds, List<OrderItem> orderItems)
        {
            // At first we load selected restaurant PriceGroup (prices are attached to price groups)
            var priceGroup = await _priceGroupRepository.GetOneNonTracking(
                p =>
                    p.Cities.Any(
                        c =>
                            c.Restaurants.Any(
                                r =>
                                    r.Id == restaurantId
                            )
                    )
            );

            // Then we load all menu items, that are present in restaurant's price group and in user's cart
            var menuItemsLoaded = await _menuItemRepository.GetManyNonTracking(
                i =>
                    i.PriceGroupsRelation.Any(
                        r =>
                            r.PriceGroupId == priceGroup.Id
                    ) && menuItemsIds.Contains(i.Id),
                i => i.PriceGroupsRelation
            );


            // Only consider menu items that belong to the selected restaurant's price group
            var menuItemsInPriceGroup = menuItemsLoaded.Where(
                i =>
                    i.PriceGroupsRelation.Any(
                        r =>
                            r.PriceGroupId == priceGroup.Id
                    )
            );

            try
            {
                foreach (var orderItem in orderItems)
                {
                    orderItem.PurchasePrice = menuItemsLoaded
                        .First(i => i.Id == orderItem.MenuItemId)
                        .PriceGroupsRelation
                        .First(r => r.PriceGroupId == priceGroup.Id)
                        .Price;
                }
            }
            catch
            {
                _logger.LogError("Failed to calculate purchase price. Cart Items Error! {restaurant_id} {client_id}", restaurantId, clientAccountId);
                throw new AkianaException("Не удалось рассчитать стоимость корзины. Обратитесь в колл-центр.");
            }
        }

        private static void CalculateAwaitedAtIfImmediate(Order order, DateTime localTimeByServer)
        {
            if (order.DelayType != OrderDelayType.Immediately) return;

            var minutes = order.PickupType switch
            {
                OrderPickupType.Pickup => 30,
                OrderPickupType.Delivery => 55,
                _ => throw new AkianaException("Неизвестный тип получения заказа")
            };

            order.AwaitedAtDateTime = localTimeByServer.AddMinutes(minutes);
        }

        private async Task<(List<OrderItem> orderItems, List<long> menuItemsIds, ICollection<CartItem> cartItems)> CloneUsersCartIntoOrderOrderItems(long clientAccountId)
        {
            // Load all user's cart items
            var cartItems = await _cartItemRepository.GetMany(
                i => i.ClientAccountId == clientAccountId,
                i => i.MenuItem
            );

            if (cartItems.Count == 0)
            {
                _logger.LogError("Empty Users Cart {client_id}", clientAccountId);
                await TelegramAPI.Send(
                    "Somehow encountered empty cart in order creation.\n" +
                    $"ClientAccount({clientAccountId})\n"
                );
                throw new AkianaException("Невозможно оформить заказ. Корзина пуста!");
            }

            // Map user's cart items to order items
            var orderItems = new List<OrderItem>();
            var menuItemsIds = new List<long>();

            foreach (var cartItem in cartItems)
            {
                for (var i = 0; i < cartItem.Amount; i++)
                {
                    orderItems.Add(
                        new()
                        {
                            MenuItemId = cartItem.MenuItemId
                        }
                    );
                }

                menuItemsIds.Add(cartItem.MenuItemId);
            }

            return (orderItems, menuItemsIds, cartItems);
        }

        private void EnsureDeliveryAddressIdIsNotPresentForPickup(OrderPickupType pickupType, long? deliveryAddressId)
        {
            if (pickupType == OrderPickupType.Pickup && deliveryAddressId is not null)
            {
                _logger.LogError("Delivery Address Present for Pickup {delivery_address}", deliveryAddressId);
                throw new AkianaException("Для самовывоза был указан адрес доставки. :(");
            }
        }

        private void EnsureThatIfDelayedAwaitedIsAfterCurrentTime(OrderDelayType delayType, DateTime? awaitedAtDateTime, DateTime localTimeByServer, TimeSpan localTimeOfDay)
        {
            if (delayType == OrderDelayType.Delayed)
            {
                if (localTimeByServer > awaitedAtDateTime)
                {
                    _logger.LogError(
                        "Delayed Order has Awaited Time After Current Time {awaited_at_datetime} {local_time_of_day} {now}",
                        awaitedAtDateTime,
                        localTimeOfDay,
                        DateTime.Now
                    );

                    throw new AkianaException("Попытка создать отложенный заказ раньше текущего времени");
                }
            }
        }

        private void EnsureThatLocalTimeIsCorrectWhenPerformingAnOrder(OrderPickupType pickupType, Restaurant restaurant, TimeSpan localTimeOfDay)
        {
            int serverDayOfWeek = GetServerDayOfWeekMondayZero();

            OpenCloseTime openCloseTime = pickupType switch
            {
                OrderPickupType.Pickup => restaurant.PickupTimes.ElementAt(serverDayOfWeek),
                OrderPickupType.Delivery => restaurant.DeliveryTimes.ElementAt(serverDayOfWeek),
                _ => throw new AkianaException("Unknown type of OrderPickupType in Mobile/Order/CreateOrderFromCart")
            };

            if (localTimeOfDay < openCloseTime.Open || localTimeOfDay > openCloseTime.Close)
            {
                _logger.LogError(
                    "Invalid hours of work {pickupType} {local_time_of_day} {open_time} {close_time} {server_time}",
                    pickupType,
                    localTimeOfDay,
                    openCloseTime.Open,
                    openCloseTime.Close,
                    DateTime.Now.TimeOfDay
                );

                throw new AkianaException("Попытка осуществить заказ в некорректное время!");
            }
        }

        private void EnsureThatRestaurantIsNotStoppedAtTheMomentOfMakingAnOrder(OrderPickupType pickupType, Restaurant restaurant)
        {
            var restaurantLastPickupStop = restaurant.PickupStops.FirstOrDefault();
            var restaurantLastDeliveryStop = restaurant.DeliveryStops.FirstOrDefault();

            switch (pickupType)
            {
                case OrderPickupType.Pickup when restaurantLastPickupStop is {EndDate: null}:
                {
                    _logger.LogError("Restaurant Can't accept Pickup Order {restaurant_id} {reason}", restaurant.Id, restaurantLastPickupStop.Reason.ToFriendlyString());
                    throw new AkianaException($"К сожалению ресторан сейчас не может принять заказ :(. Причина: {restaurantLastPickupStop.Reason.ToFriendlyString()}");
                }
                case OrderPickupType.Delivery when restaurantLastDeliveryStop is {EndDate: null}:
                {
                    _logger.LogError("Restaurant Can't accept Delivery Order {restaurant_id} {reason}", restaurant.Id, restaurantLastDeliveryStop.Reason.ToFriendlyString());
                    throw new AkianaException($"К сожалению ресторан сейчас не может принять заказ :(. Причина: {restaurantLastDeliveryStop.Reason.ToFriendlyString()}");
                }
                default: break;
            }
        }

        private static int GetServerDayOfWeekMondayZero()
        {
            return ((int) DateTime.Now.DayOfWeek + 7 - 1) % 7;
        }

        public async Task<OrderMobileDto> GetById(long id)
        {
            var order = await _orderRepository.GetByIdForMobile(id);

            order.EnsureNotNullHandled("Заказ не найден.");

            var gmtOffset = await _cityRepository.GetGmtOffsetByOrder(order.Id);

            order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffset));
            order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);

            var orderMobileDto = _mapper.Map<OrderMobileDto>(order);

            return orderMobileDto;
        }

        public async Task<ICollection<OrderMobileDto>> GetByClient(long id, int offset, int limit)
        {
            var orders = await _orderRepository.GetManyReversedNonTracking(
                o => o.ClientAccountId == id &&
                     (o.PaymentType != PaymentTypeFlags.Online ||
                      (o.PaymentType == PaymentTypeFlags.Online && o.OnlinePayment.PaymentStatus == PaymentStatus.Payed)
                     ),
                offset,
                limit,
                o => o.OrderItems
            );

            var gmtOffsetsByOrders = await _cityRepository.GetGmtOffsetsByOrders(
                orders.Select(o => o.Id)
                    .ToList()
            );

            foreach (var order in orders)
            {
                order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffsetsByOrders[order.Id]));
                order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);
            }

            var orderMobileDtos = _mapper.Map<ICollection<OrderMobileDto>>(orders);

            return orderMobileDtos;
        }

        public async Task<ICollection<OrderMobileDto>> GetByAddress(long id)
        {
            var orders = await _orderRepository.GetManyReversedNonTracking(
                o => o.DeliveryAddressId == id,
                o => o.OrderItems
            );

            var gmtOffsetsByOrders = await _cityRepository.GetGmtOffsetsByOrders(
                orders.Select(o => o.Id)
                    .ToList()
            );

            foreach (var order in orders)
            {
                order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffsetsByOrders[order.Id]));
                order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);
            }

            var orderMobileDtos = _mapper.Map<ICollection<OrderMobileDto>>(orders);

            return orderMobileDtos;
        }

        public async Task<ICollection<OrderMobileDto>> GetByClientAndRestaurant(long clientId, long restaurantId)
        {
            var orders = await _orderRepository.GetManyReversedNonTracking(
                o => o.ClientAccountId == clientId && o.RestaurantId == restaurantId,
                o => o.OrderItems
            );

            var gmtOffsetsByOrders = await _cityRepository.GetGmtOffsetsByOrders(
                orders.Select(o => o.Id)
                    .ToList()
            );

            foreach (var order in orders)
            {
                order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffsetsByOrders[order.Id]));
                order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);
            }

            var orderMobileDtos = _mapper.Map<ICollection<OrderMobileDto>>(orders);

            return orderMobileDtos;
        }

        public async Task TryConfirmOrderPayment(long orderId)
        {
            OnlinePayment onlinePayment;

            int tries = 1;
            do
            {
                onlinePayment = await _onlinePaymentRepository.GetByOrder(orderId);

                if (onlinePayment.PaymentStatus == PaymentStatus.Payed)
                {
                    break;
                }

                await Task.Delay(1000);
                tries++;
            } while (tries <= 3);

            if (onlinePayment.PaymentStatus != PaymentStatus.Payed)
            {
                SberbankGetOrderStatusResultDto sberbankGetOrderStatusResultDto = null;
                var restaurant = await _restaurantRepository.GetOneNonTracking(r => r.Orders.Any(o => o.Id == orderId));

                if (restaurant is null)
                {
                    _logger.LogCritical("Restaurant was not found for order {order_id}", orderId);
                    await PayError.RestaurantNotFound(orderId)
                        .Throw(onlinePayment.Order.ClientAccountId, onlinePayment.Order.ClientAccount.Login, onlinePayment.IssuerId);
                }

                if (string.IsNullOrEmpty(restaurant.SberbankUsername) || string.IsNullOrEmpty(restaurant.SberbankPassword))
                {
                    _logger.LogError(
                        "No Username Or Password Specified for Restaurant {restaurant_id} {login} {password}",
                        restaurant.Id,
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword
                    );
                    await PayError.NoUsernameOrPassword(restaurant.Id)
                        .Throw(onlinePayment.Order.ClientAccountId, onlinePayment.Order.ClientAccount.Login, onlinePayment.IssuerId);
                }

                try
                {
                    // если не получили Callback - идём за статусом сами
                    sberbankGetOrderStatusResultDto = await _paymentService.GetStatus(restaurant.SberbankUsername, restaurant.SberbankPassword, onlinePayment.ExternalId);
                }
                catch (AkianaException ex)
                {
                    _logger.LogError(
                        ex,
                        "Sberbank GetStatus Failed {restaurant_id} {result}",
                        restaurant.Id,
                        sberbankGetOrderStatusResultDto
                    );
                    await PayError.SberbankGetOrderStatusExtendedUnavailable
                        .Throw(onlinePayment.Order.ClientAccountId, onlinePayment.Order.ClientAccount.Login, onlinePayment.IssuerId);
                }

                if (sberbankGetOrderStatusResultDto.ErrorCode != "0")
                {
                    _logger.LogError(
                        "Sberbank ErrorCode was not null {restaurant_id} {login} {password} {response}",
                        restaurant.Id,
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword,
                        sberbankGetOrderStatusResultDto
                    );
                    await TelegramAPI.Send(
                        $"Mobile/TryConfirmOrderPayment Error\n" +
                        $"Sberbank returned unknown ErrorCode: {sberbankGetOrderStatusResultDto.ErrorCode}\n" +
                        sberbankGetOrderStatusResultDto.ErrorMessage
                    );

                    await PayError.SberbankUnknownErrorCode(sberbankGetOrderStatusResultDto.ErrorCode)
                        .Throw(onlinePayment.Order.ClientAccountId, onlinePayment.Order.ClientAccount.Login, onlinePayment.IssuerId);
                }

                if (sberbankGetOrderStatusResultDto.OrderStatus == 2)
                {
                    // оплата прошла успешно, мы просто не получили Callback
                    onlinePayment.ConfirmationSource = ConfirmationSource.CustomCall;
                    onlinePayment.ConfirmedAt = DateTime.Now;
                    onlinePayment.PaymentStatus = PaymentStatus.Payed;
                    await _onlinePaymentRepository.Update(onlinePayment);

                    var cartItemsFromOnlinePayment = await _cartItemRepository.GetCartItemsFromOnlinePaymentId(onlinePayment.Id);

                    // Clear user's cart
                    await _cartItemRepository.RemoveMany(cartItemsFromOnlinePayment);

                    await _orderPostProcessingQueue.Enqueue(onlinePayment.IssuerId, onlinePayment.OrderId);
                }
                else
                {
                    onlinePayment.ConfirmationSource = ConfirmationSource.CustomCall;
                    onlinePayment.ConfirmedAt = DateTime.Now;
                    onlinePayment.PaymentStatus = PaymentStatus.Declined;
                    await _onlinePaymentRepository.Update(onlinePayment);

                    _logger.LogError(
                        "Sberbank Unknown OrderStatus {restaurant_id} {login} {password} {status}",
                        restaurant.Id,
                        restaurant.SberbankUsername,
                        restaurant.SberbankPassword,
                        sberbankGetOrderStatusResultDto.OrderStatus
                    );

                    await PayError.SberbankUnknownOrderStatus(sberbankGetOrderStatusResultDto.OrderStatus)
                        .Throw(onlinePayment.Order.ClientAccountId, onlinePayment.Order.ClientAccount.Login, onlinePayment.IssuerId);
                }
            }
        }
    }
}