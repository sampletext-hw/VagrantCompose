using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Models.Db.DbRestaurant;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations
{
    public class RestaurantByCityCacheService : IRestaurantByCityCacheService
    {
        private SemaphoreSlim _semaphore;

        private Dictionary<long, ICollection<Restaurant>> _cache;

        private IServiceProvider _serviceProvider;

        public RestaurantByCityCacheService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _semaphore = new SemaphoreSlim(1, 1);
            _cache = new();
        }

        public async Task<ICollection<Restaurant>> GetByCity(long cityId)
        {
            await EnsureExistence(cityId);

            return _cache[cityId];
        }

        private async Task EnsureExistence(long cityId)
        {
            if (!_cache.ContainsKey(cityId))
            {
                await _semaphore.WaitAsync();
                if (!_cache.ContainsKey(cityId))
                {
                    await LoadRestaurants(cityId);
                }

                _semaphore.Release();
            }
        }

        public async Task UpdateAllByCity(long cityId)
        {
            await _semaphore.WaitAsync();
            await LoadRestaurants(cityId);
            _semaphore.Release();
        }

        public async Task UpdateRestaurant(long restaurantId)
        {
            await _semaphore.WaitAsync();
            
            Restaurant restaurant = null;
            long cityId = 0;

            if (_cache.Any(kv =>
            {
                var (key, value) = kv;
                cityId = key;
                restaurant = value.FirstOrDefault(r => r.Id == restaurantId);
                return restaurant != null;
            }))
            {
                using var serviceScope = _serviceProvider.CreateScope();
                var restaurantRepository = serviceScope.ServiceProvider.GetRequiredService<IRestaurantRepository>();
                var loadedRestaurant = await restaurantRepository.GetByIdNonTracking(
                    restaurant.Id,
                    r => r.City,
                    r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                    r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                    r => r.Location,
                    r => r.DeliveryZone.OrderBy(z => z.Order),
                    r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                    r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
                );

                _cache[cityId].Remove(restaurant);
                _cache[cityId].Add(loadedRestaurant);
            }
            else
            {
                // If restaurant is not loaded to cache yet, then we can omit the operation. It will be loaded on city demand
            }
            
            _semaphore.Release();
        }

        private async Task LoadRestaurants(long cityId)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var restaurantRepository = serviceScope.ServiceProvider.GetRequiredService<IRestaurantRepository>();
            var restaurants = await restaurantRepository.GetManyNonTracking(
                r => r.CityId == cityId,
                r => r.City,
                r => r.PickupTimes.OrderBy(t => t.DayOfWeek),
                r => r.DeliveryTimes.OrderBy(t => t.DayOfWeek),
                r => r.Location,
                r => r.DeliveryZone.OrderBy(z => z.Order),
                r => r.PickupStops.OrderByDescending(s => s.Id).Take(1),
                r => r.DeliveryStops.OrderByDescending(s => s.Id).Take(1)
            );

            _cache[cityId] = restaurants;
        }
    }
}