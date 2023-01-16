using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.RestaurantStop;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;
using Models.Misc;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;
using Services.FranchiseeServices.Abstractions;
using Services.Shared.Abstractions;

namespace Services.FranchiseeServices.Implementations
{
    public class RestaurantStopService : IRestaurantStopService
    {
        private readonly IRestaurantPickupStopRepository _restaurantPickupStopRepository;
        private readonly IRestaurantDeliveryStopRepository _restaurantDeliveryStopRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IRequestAccountIdService _requestAccountIdService;
        private readonly IRestaurantByCityCacheService _restaurantByCityCacheService;

        public RestaurantStopService(IRestaurantPickupStopRepository restaurantPickupStopRepository, IRestaurantDeliveryStopRepository restaurantDeliveryStopRepository, IMapper mapper, IRestaurantRepository restaurantRepository, IRequestAccountIdService requestAccountIdService, IRestaurantByCityCacheService restaurantByCityCacheService)
        {
            _restaurantPickupStopRepository = restaurantPickupStopRepository;
            _restaurantDeliveryStopRepository = restaurantDeliveryStopRepository;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _requestAccountIdService = requestAccountIdService;
            _restaurantByCityCacheService = restaurantByCityCacheService;
        }

        public async Task<CreatedDto> CreatePickupStop(CreateRestaurantStopDto createRestaurantStopDto)
        {
            var franchiseeId = _requestAccountIdService.Id;
            
            if (!await _restaurantRepository.CanBeManagedBy(createRestaurantStopDto.RestaurantId, franchiseeId))
            {
                await TelegramAPI.Send($"Franchisee/RestaurantStop/CreatePickupStop\nAttempt to access Restaurant({createRestaurantStopDto.RestaurantId}) by WorkerAccount({franchiseeId})");
                throw new AkianaException("У вас нет доступа к этому суши-бару");
            }
            
            var lastRestaurantPickupStop = await _restaurantPickupStopRepository.GetLastNonTracking(s => s.RestaurantId == createRestaurantStopDto.RestaurantId);

            if (lastRestaurantPickupStop is {EndDate: null})
            {
                throw new AkianaException("У этого ресторана уже остановлен самовывоз");
            }

            var restaurantPickupStop = _mapper.Map<RestaurantPickupStop>(createRestaurantStopDto);
            restaurantPickupStop.IssuerId = franchiseeId;
            restaurantPickupStop.StartDate = DateTime.Now;
            
            await _restaurantPickupStopRepository.Add(restaurantPickupStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(createRestaurantStopDto.RestaurantId);
            
            return restaurantPickupStop.Id;
        }

        public async Task<CreatedDto> CreateDeliveryStop(CreateRestaurantStopDto createRestaurantStopDto)
        {
            var franchiseeId = _requestAccountIdService.Id;
            
            if (!await _restaurantRepository.CanBeManagedBy(createRestaurantStopDto.RestaurantId, franchiseeId))
            {
                await TelegramAPI.Send($"Franchisee/RestaurantStop/CreateDeliveryStop\nAttempt to access Restaurant({createRestaurantStopDto.RestaurantId}) by WorkerAccount({franchiseeId})");
                throw new AkianaException("У вас нет доступа к этому суши-бару");
            }
            
            var lastRestaurantDeliveryStop = await _restaurantDeliveryStopRepository.GetLastNonTracking(s => s.RestaurantId == createRestaurantStopDto.RestaurantId);

            if (lastRestaurantDeliveryStop is {EndDate: null})
            {
                throw new AkianaException("У этого ресторана уже остановлена доставка");
            }

            var restaurantDeliveryStop = _mapper.Map<RestaurantDeliveryStop>(createRestaurantStopDto);
            restaurantDeliveryStop.IssuerId = franchiseeId;
            restaurantDeliveryStop.StartDate = DateTime.Now;
            
            await _restaurantDeliveryStopRepository.Add(restaurantDeliveryStop);
            
            await _restaurantByCityCacheService.UpdateRestaurant(createRestaurantStopDto.RestaurantId);
            
            return restaurantDeliveryStop.Id;
        }

        public async Task FinishPickupStop(long restaurantId)
        {
            var franchiseeId = _requestAccountIdService.Id;
            if (!await _restaurantRepository.CanBeManagedBy(restaurantId, franchiseeId))
            {
                await TelegramAPI.Send($"Franchisee/RestaurantStop/FinishPickupStop\nAttempt to access Restaurant({restaurantId}) by WorkerAccount({franchiseeId})");
                throw new AkianaException("У вас нет доступа к этому суши-бару");
            }
            
            var restaurantPickupStop = await _restaurantPickupStopRepository.GetLast(
                s => s.RestaurantId == restaurantId
            );

            restaurantPickupStop.EnsureNotNullHandled("У этого ресторана никогда не было остановок самовывоза");

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
            var franchiseeId = _requestAccountIdService.Id;
            if (!await _restaurantRepository.CanBeManagedBy(restaurantId, franchiseeId))
            {
                await TelegramAPI.Send($"Franchisee/RestaurantStop/FinishDeliveryStop\nAttempt to access Restaurant({restaurantId}) by WorkerAccount({franchiseeId})");
                throw new AkianaException("У вас нет доступа к этому суши-бару");
            }
            
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