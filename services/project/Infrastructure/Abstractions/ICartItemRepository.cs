using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.DbCart;

namespace Infrastructure.Abstractions
{
    using T = CartItem;

    public interface ICartItemRepository : IAdd<T>, IAddMany<T>, IGetOne<T>, IUpdate<T>, IRemove<T>, IRemoveMany<T>, IGetById<T>, IGetMany<T>
    {
        Task<ICollection<T>> GetCartItemsFromOnlinePaymentId(long onlinePaymentId);
    }
}