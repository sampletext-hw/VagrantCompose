using Infrastructure.BaseAbstractions;
using Models.Db.RestaurantStop;

namespace Infrastructure.Abstractions
{
    using T = RestaurantDeliveryStop;

    public interface IRestaurantDeliveryStopRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, IGetOne<T>, IGetById<T>, IGetLast<T>
    {
    }
}