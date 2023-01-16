using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.Restaurants;
using Services.CommonServices.Abstractions;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantByCityCacheService _restaurantByCityCacheService;

        private readonly IMapper _mapper;

        public RestaurantService(IRestaurantByCityCacheService restaurantByCityCacheService, IMapper mapper)
        {
            _restaurantByCityCacheService = restaurantByCityCacheService;
            _mapper = mapper;
        }

        public async Task<ICollection<RestaurantMobileDto>> GetByCity(long cityId)
        {
            var restaurants = await _restaurantByCityCacheService.GetByCity(cityId);

            var restaurantMobileDtos = _mapper.Map<ICollection<RestaurantMobileDto>>(restaurants);

            return restaurantMobileDtos;
        }
    }
}