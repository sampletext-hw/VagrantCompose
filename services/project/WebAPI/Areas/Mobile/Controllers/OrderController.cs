using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db;
using Models.Db.Account;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.Payments;
using Models.DTOs.Misc;
using Models.DTOs.Orders;
using Services.CommonServices.Abstractions;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class OrderController : AkianaMobileController
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию о заказе (с шифрацией)")]
        public async Task<ActionResult<OrderMobileDto>> GetById([Id(typeof(Order))] long id)
        {
            var orderMobileDto = await _orderService.GetById(id);
            return Ok(orderMobileDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Создаёт заказ (с шифрацией)")]
        public async Task<ActionResult<CreatedDto>> CreateFromCart([ModelBinder(typeof(EncodedJsonBinder))] MobileCreateOrderFromCartDto mobileCreateOrderFromCartDto)
        {
            var createdDto = await _orderService.CreateFromCart(mobileCreateOrderFromCartDto);
            return Ok(createdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Создаёт заказ V2 с возможность оплаты онлайн (с шифрацией)")]
        public async Task<ActionResult<OrderCreationResultDto>> CreateFromCartV2([ModelBinder(typeof(EncodedJsonBinder))] MobileCreateOrderFromCartV2Dto mobileCreateOrderFromCartDto)
        {
            var orderCreationResultDto = await _orderService.CreateFromCartV2(mobileCreateOrderFromCartDto);
            return Ok(orderCreationResultDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает список заказов пользователя (с шифрацией)")]
        public async Task<ActionResult<ICollection<OrderMobileDto>>> GetByClient([Id(typeof(ClientAccount))] long id, int offset = 0, int limit = 25)
        {
            var orderMobileDtos = await _orderService.GetByClient(id, offset, limit);
            return Ok(orderMobileDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает список заказов на адрес доставки (с шифрацией)")]
        public async Task<ActionResult<ICollection<OrderMobileDto>>> GetByAddress([Id(typeof(DeliveryAddress))] long id)
        {
            var orderMobileDtos = await _orderService.GetByAddress(id);
            return Ok(orderMobileDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает список заказов клиента в конкретной точке (с шифрацией)")]
        public async Task<ActionResult<ICollection<OrderMobileDto>>> GetByClientAndRestaurant([Id(typeof(ClientAccount))] long clientId, [Id(typeof(Restaurant))] long restaurantId)
        {
            var orderMobileDtos = await _orderService.GetByClientAndRestaurant(clientId, restaurantId);
            return Ok(orderMobileDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Подтверждает оплату заказа (с шифрацией)")]
        public async Task<ActionResult> TryConfirmOrderPayment([Id(typeof(Order))] long id)
        {
            await _orderService.TryConfirmOrderPayment(id);
            return Ok();
        }

        // [HttpGet]
        // public async Task<ActionResult> Test(long orderId, float total)
        // {
        //     var result = await _paymentService.CreatePayment(TODO, TODO, orderId, total, "Супер тест", 12);
        //
        //     return Ok(result);
        // }
        //
        // [HttpGet]
        // public async Task<ActionResult> GetStatus(string externalId)
        // {
        //     var result = await _paymentService.GetStatus(TODO, TODO, externalId);
        //
        //     return Ok(result);
        // }
    }
}