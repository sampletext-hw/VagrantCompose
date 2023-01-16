using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.RestaurantStop;

namespace Infrastructure.Implementations
{
    public class RestaurantPickupStopRepository : IdRepositoryBase<RestaurantPickupStop>, IRestaurantPickupStopRepository
    {
        public RestaurantPickupStopRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}