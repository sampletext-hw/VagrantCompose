using Infrastructure.BaseAbstractions;
using Models.Db.RestaurantStop;

namespace Infrastructure.Abstractions
{
    using T = RestaurantPickupStop;

    public interface IRestaurantPickupStopRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, IGetOne<T>, IGetById<T>, IGetLast<T>
    {
    }
}