using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;

namespace Services.SuperuserServices.Abstractions
{
    public interface IRestaurantStopService
    {
        Task<ICollection<RestaurantStopDto>> GetPickupByRestaurant(long id);
        Task<ICollection<RestaurantStopDto>> GetDeliveryByRestaurant(long id);
        Task<CreatedDto> CreatePickupStop(CreateRestaurantStopDto createRestaurantStopDto);
        Task<CreatedDto> CreateDeliveryStop(CreateRestaurantStopDto createRestaurantStopDto);
        Task FinishPickupStop(long restaurantId);
        Task FinishDeliveryStop(long restaurantId);
    }
}