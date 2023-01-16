using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db;
using Models.Db.DbRestaurant;

namespace Infrastructure.Abstractions
{
    using T = City;

    public interface ICityRepository : IGetById<T>, IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, ICount<T>
    {
        Task<int> GetGmtOffsetByRestaurant(long restaurantId);
        
        Task<int> GetGmtOffsetByOrder(long orderId);

        Task<Dictionary<long, int>> GetGmtOffsetsByOrders(ICollection<long> orderIds);
    }
}