using System.Threading.Tasks;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;

namespace Services.FranchiseeServices.Abstractions
{
    public interface IRestaurantStopService
    {
        Task<CreatedDto> CreatePickupStop(CreateRestaurantStopDto createRestaurantStopDto);
        Task<CreatedDto> CreateDeliveryStop(CreateRestaurantStopDto createRestaurantStopDto);
        Task FinishPickupStop(long restaurantId);
        Task FinishDeliveryStop(long restaurantId);
    }
}