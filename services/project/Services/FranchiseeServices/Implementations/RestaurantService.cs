using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.Restaurants;
using Models.Misc;
using Services.ExternalServices;
using Services.FranchiseeServices.Abstractions;
using Services.Shared.Abstractions;

namespace Services.FranchiseeServices.Implementations
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IRequestAccountIdService _requestAccountIdService;

        public RestaurantService(IRestaurantRepository restaurantRepository, IMapper mapper, IRequestAccountIdService requestAccountIdService)
        {
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _requestAccountIdService = requestAccountIdService;
        }

        public async Task<ICollection<RestaurantWithIdDto>> GetByWorker(long id)
        {
            var restaurants = await _restaurantRepository.GetManyNonTracking(
                r => r.WorkerAccountsRelation.Any(rel => rel.WorkerAccountId == id),
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

        public async Task<ICollection<RestaurantWithIdDto>> GetMy()
        {
            var franchiseeId = _requestAccountIdService.Id;
            
            var restaurants = await _restaurantRepository.GetManyNonTracking(
                r => r.WorkerAccountsRelation.Any(rel => rel.WorkerAccountId == franchiseeId),
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

        public async Task<RestaurantWithIdDto> GetById(long id)
        {
            var franchiseeId = _requestAccountIdService.Id;

            if (!await _restaurantRepository.CanBeManagedBy(id, franchiseeId))
            {
                await TelegramAPI.Send($"Franchisee/Restaurant/GetById\nAttempt to access Restaurant({id}) by WorkerAccount({franchiseeId})");
                throw new AkianaException("У вас нет доступа к этому суши-бару");
            }

            var restaurant = await _restaurantRepository.GetOneNonTracking(
                r => r.Id == id,
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
    }
}