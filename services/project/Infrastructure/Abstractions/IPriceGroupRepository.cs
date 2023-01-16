using Infrastructure.BaseAbstractions;
using Models.Db;
using Models.Db.DbRestaurant;

namespace Infrastructure.Abstractions
{
    using T = PriceGroup;

    public interface IPriceGroupRepository : IGetOne<T>, IGetById<T>, IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, ICount<T>
    {
    }
}