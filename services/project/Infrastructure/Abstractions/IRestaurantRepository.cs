using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.DbRestaurant;

namespace Infrastructure.Abstractions
{
    using T = Restaurant;

    public interface IRestaurantRepository : IGetById<T>, IGetOne<T>, IAdd<T>, IUpdate<T>, IRemove<T>, IGetMany<T>, ICount<T>
    {
        Task<bool> CanBeManagedBy(long id, long workerId);
    }
}