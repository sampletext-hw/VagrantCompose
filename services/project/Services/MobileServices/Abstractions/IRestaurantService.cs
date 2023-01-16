using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Restaurants;

namespace Services.MobileServices.Abstractions
{
    public interface IRestaurantService
    {
        Task<ICollection<RestaurantMobileDto>> GetByCity(long cityId);
    }
}