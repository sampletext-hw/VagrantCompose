using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Restaurants;

namespace Services.FranchiseeServices.Abstractions
{
    public interface IRestaurantService
    {
        Task<ICollection<RestaurantWithIdDto>> GetByWorker(long id);
        
        Task<ICollection<RestaurantWithIdDto>> GetMy();

        Task<RestaurantWithIdDto> GetById(long id);
    }
}