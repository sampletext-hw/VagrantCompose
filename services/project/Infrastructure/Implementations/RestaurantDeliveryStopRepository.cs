using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.RestaurantStop;

namespace Infrastructure.Implementations
{
    public class RestaurantDeliveryStopRepository : IdRepositoryBase<RestaurantDeliveryStop>, IRestaurantDeliveryStopRepository
    {
        public RestaurantDeliveryStopRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}