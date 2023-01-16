using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.RestaurantStop;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;
using Models.Misc;
using Services.CommonServices.Abstractions;
using Services.Shared.Abstractions;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class RestaurantStopService : IRestaurantStopService
    {
        private readonly IRestaurantPickupStopRepository _restaurantPickupStopRepository;
        private readonly IRestaurantDeliveryStopRepository _restaurantDeliveryStopRepository;

        private readonly IMapper _mapper;
        private readonly IRequestAccountIdService _requestAccountIdService;
        private IRestaurantByCityCacheService _restaurantByCityCacheService;

        public RestaurantStopService(IRestaurantPickupStopRepository restaurantPickupStopRepository, IRestaurantDeliveryStopRepository restaurantDeliveryStopRepository, IMapper mapper, IRequestAccountIdService requestAccountIdService, IRestaurantByCityCacheService restaurantByCityCacheService)
        {
            _restaurantPickupStopRepository = restaurantPickupStopRepository;
            _restaurantDeliveryStopRepository = restaurantDeliveryStopRepository;
            _mapper = mapper;
            _requestAccountIdService = requestAccountIdService;
            _restaurantByCityCacheService = restaurantByCityCacheService;
        }

        public async Task<ICollection<RestaurantStopDto>> GetPickupByRestaurant(long id)
        {
            var restaurantPickupStops = await _restaurantPickupStopRepository.GetManyNonTracking(s => s.RestaurantId == id);

            var restaurantStopDtos = _mapper.Map<ICollection<RestaurantStopDto>>(restaurantPickupStops);

            return restaurantStopDtos;
        }

        public async Task<ICollection<RestaurantStopDto>> GetDeliveryByRestaurant(long id)
        {
            var restaurantDeliveryStops = await _restaurantDeliveryStopRepository.GetManyNonTracking(s => s.RestaurantId == id);

            var restaurantStopDtos = _mapper.Map<ICollection<RestaurantStopDto>>(restaurantDeliveryStops);

            return restaurantStopDtos;
        }

        public async Task<CreatedDto> CreatePickupStop(CreateRestaurantStopDto createRestaurantStopDto)
        {
            var lastRestaurantPickupStop = await _restaurantPickupStopRepository.GetLastNonTracking(s => s.RestaurantId == createRestaurantStopDto.RestaurantId);

            if (lastRestaurantPickupStop is {EndDate: null})
            {
                throw new AkianaException("У этого ресторана уже остановлен самовывоз");
            }
            
            long issuerId = _requestAccountIdService.Id;
            var restaurantPickupStop = _mapper.Map<RestaurantPickupStop>(createRestaurantStopDto);
            restaurantPickupStop.IssuerId = issuerId;
            restaurantPickupStop.StartDate = DateTime.Now;
            await _restaurantPickupStopRepository.Add(restaurantPickupStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(createRestaurantStopDto.RestaurantId);
            
            return restaurantPickupStop.Id;
        }

        public async Task<CreatedDto> CreateDeliveryStop(CreateRestaurantStopDto createRestaurantStopDto)
        {
            var lastRestaurantDeliveryStop = await _restaurantDeliveryStopRepository.GetLastNonTracking(s => s.RestaurantId == createRestaurantStopDto.RestaurantId);

            if (lastRestaurantDeliveryStop is {EndDate: null})
            {
                throw new AkianaException("У этого ресторана уже остановлена доставка");
            }

            long issuerId = _requestAccountIdService.Id;
            
            var restaurantDeliveryStop = _mapper.Map<RestaurantDeliveryStop>(createRestaurantStopDto);
            restaurantDeliveryStop.IssuerId = issuerId;
            restaurantDeliveryStop.StartDate = DateTime.Now;
            await _restaurantDeliveryStopRepository.Add(restaurantDeliveryStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(createRestaurantStopDto.RestaurantId);
            return restaurantDeliveryStop.Id;
        }

        public async Task FinishPickupStop(long restaurantId)
        {
            var restaurantPickupStop = await _restaurantPickupStopRepository.GetLast(s => s.RestaurantId == restaurantId);

            if (restaurantPickupStop is not {EndDate: null})
            {
                throw new AkianaException("Самовывоз в данный момент не остановлен");
            }
            
            restaurantPickupStop.EndDate = DateTime.Now;
            await _restaurantPickupStopRepository.Update(restaurantPickupStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(restaurantId);
        }

        public async Task FinishDeliveryStop(long restaurantId)
        {
            var restaurantDeliveryStop = await _restaurantDeliveryStopRepository.GetLast(s => s.RestaurantId == restaurantId);

            restaurantDeliveryStop.EnsureNotNullHandled("У этого ресторана никогда не было остановок доставок");

            if (restaurantDeliveryStop is not {EndDate: null})
            {
                throw new AkianaException("Доставка в данный момент не остановлена");
            }
            
            restaurantDeliveryStop.EndDate = DateTime.Now;
            await _restaurantDeliveryStopRepository.Update(restaurantDeliveryStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(restaurantId);
        }
    }
}