using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Db.DbRestaurant;

namespace Services.CommonServices.Abstractions
{
    public interface IRestaurantByCityCacheService
    {
        public Task<ICollection<Restaurant>> GetByCity(long cityId);

        public Task UpdateAllByCity(long cityId);
        
        public Task UpdateRestaurant(long restaurantId);
    }
}