using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;
using Models.DTOs.Restaurants;
using Models.Misc;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ICityRepository _cityRepository;

        private readonly IDeliveryZoneLatLngRepository _deliveryZoneLatLngRepository;

        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IWorkerAccountRepository _workerAccountRepository;
        private IRestaurantByCityCacheService _restaurantByCityCacheService;

        public RestaurantService(IRestaurantRepository restaurantRepository, ICityRepository cityRepository, IMapper mapper, IWorkerAccountRepository workerAccountRepository, IDeliveryZoneLatLngRepository deliveryZoneLatLngRepository, IRestaurantByCityCacheService restaurantByCityCacheService)
        {
            _restaurantRepository = restaurantRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
            _workerAccountRepository = workerAccountRepository;
            _deliveryZoneLatLngRepository = deliveryZoneLatLngRepository;
            _restaurantByCityCacheService = restaurantByCityCacheService;
        }

        public async Task<RestaurantWithIdDto> GetById(long id)
        {
            var restaurant = await _restaurantRepository.GetByIdNonTracking(id,
                r => r.City,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.Location,
                r => r.DeliveryZone.OrderBy(z => z.Order),
                r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
            );

            var restaurantWithIdDto = _mapper.Map<RestaurantWithIdDto>(restaurant);

            return restaurantWithIdDto;
        }

        public async Task<ICollection<RestaurantWithIdDto>> GetAll()
        {
            var restaurants = await _restaurantRepository.GetManyNonTracking(null,
                r => r.City,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.Location,
                r => r.DeliveryZone.OrderBy(z => z.Order),
                r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
            );

            var restaurantWithIdDtos = _mapper.Map<ICollection<RestaurantWithIdDto>>(restaurants);

            return restaurantWithIdDtos;
        }

        public async Task<ICollection<RestaurantWithIdDto>> GetByCity(long id)
        {
            var restaurants = await _restaurantRepository.GetManyNonTracking(
                r => r.CityId == id,
                r => r.City,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.Location,
                r => r.DeliveryZone.OrderBy(z => z.Order),
                r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
            );

            var restaurantWithIdDtos = _mapper.Map<ICollection<RestaurantWithIdDto>>(restaurants);

            return restaurantWithIdDtos;
        }

        public async Task Update(UpdateRestaurantDto updateRestaurantDto)
        {
            EnsureOpenCloseTimeCorrect(updateRestaurantDto);

            if (updateRestaurantDto.SupportedPaymentTypes.Count == 0)
            {
                throw new AkianaException("???? ???????????? ???? ???????? ?????? ???????????? ??????????????????.");
            }

            var restaurant = await _restaurantRepository.GetById(updateRestaurantDto.Id,
                r => r.City,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.Location,
                r => r.DeliveryZone.OrderBy(z => z.Order),
                r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
            );

            updateRestaurantDto.PickupTimes = updateRestaurantDto.PickupTimes.OrderBy(t => t.DayOfWeek).ToList();
            updateRestaurantDto.DeliveryTimes = updateRestaurantDto.DeliveryTimes.OrderBy(t => t.DayOfWeek).ToList();
            updateRestaurantDto.DeliveryZone = updateRestaurantDto.DeliveryZone.OrderBy(z => z.Order).ToList();

            var oldZoneLatLngs = await _deliveryZoneLatLngRepository.GetMany(z => z.RestaurantId == updateRestaurantDto.Id);

            await _deliveryZoneLatLngRepository.RemoveMany(oldZoneLatLngs);

            _mapper.Map(updateRestaurantDto, restaurant);

            await _restaurantRepository.Update(restaurant);

            await _restaurantByCityCacheService.UpdateRestaurant(restaurant.Id);
        }

        public async Task<CreatedDto> Create(CreateRestaurantDto createRestaurantDto)
        {
            EnsureOpenCloseTimeCorrect(createRestaurantDto);

            if (createRestaurantDto.SupportedPaymentTypes.Count == 0)
            {
                throw new AkianaException("???? ???????????? ???? ???????? ?????? ???????????? ??????????????????.");
            }

            var restaurant = _mapper.Map<Restaurant>(createRestaurantDto);

            await _restaurantRepository.Add(restaurant);

            await _restaurantByCityCacheService.UpdateAllByCity(restaurant.CityId);

            return restaurant.Id;
        }

        public async Task Remove(long id)
        {
            var restaurant = await _restaurantRepository.GetById(
                id,
                r => r.WorkerAccountsRelation
            );

            if (restaurant.WorkerAccountsRelation.Any()) throw new AkianaException("???????????????????? ?????????????? ???????? ????????????????! ?? ???????? ?????? ?????????????????? ??????????????????");

            await _restaurantRepository.Remove(restaurant);

            await _restaurantByCityCacheService.UpdateAllByCity(restaurant.CityId);
        }

        private static void EnsureOpenCloseTimeCorrect(UpdateRestaurantDto updateRestaurantDto)
        {
            if (updateRestaurantDto.PickupTimes.Count != 7) throw new AkianaException("\"pickupTimes\" ???????????? ?????????????????? 7 ??????????????????!");

            if (updateRestaurantDto.DeliveryTimes.Count != 7) throw new AkianaException("\"deliveryTimes\" ???????????? ?????????????????? 7 ??????????????????!");

            foreach (var openCloseTimeDto in updateRestaurantDto.DeliveryTimes)
                if (openCloseTimeDto.Open > openCloseTimeDto.Close)
                    throw new AkianaException($"?????????? ???????????????? ?????????????? ?????????????????????? ?? ({openCloseTimeDto.DayOfWeek}) ????????");

            foreach (var openCloseTimeDto in updateRestaurantDto.PickupTimes)
                if (openCloseTimeDto.Open > openCloseTimeDto.Close)
                    throw new AkianaException($"?????????? ???????????????????? ?????????????? ?????????????????????? ?? ({openCloseTimeDto.DayOfWeek}) ????????");
        }


        private static void EnsureOpenCloseTimeCorrect(CreateRestaurantDto createRestaurantDto)
        {
            if (createRestaurantDto.PickupTimes.Count != 7) throw new AkianaException("\"pickupTimes\" ???????????? ?????????????????? 7 ??????????????????!");

            if (createRestaurantDto.DeliveryTimes.Count != 7) throw new AkianaException("\"deliveryTimes\" ???????????? ?????????????????? 7 ??????????????????!");

            foreach (var openCloseTimeDto in createRestaurantDto.DeliveryTimes)
                if (openCloseTimeDto.Open > openCloseTimeDto.Close)
                    throw new AkianaException($"?????????? ???????????????? ?????????????? ?????????????????????? ?? ({openCloseTimeDto.DayOfWeek}) ????????");

            foreach (var openCloseTimeDto in createRestaurantDto.PickupTimes)
                if (openCloseTimeDto.Open > openCloseTimeDto.Close)
                    throw new AkianaException($"?????????? ???????????????????? ?????????????? ?????????????????????? ?? ({openCloseTimeDto.DayOfWeek}) ????????");
        }
    }
}