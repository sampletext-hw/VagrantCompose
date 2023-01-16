using Infrastructure.BaseAbstractions;
using Models.Db.DbCart;

namespace Infrastructure.Abstractions
{
    using T = FavoriteItem;

    public interface IFavoriteItemRepository : IAdd<T>, IAddMany<T>, IGetOne<T>, IUpdate<T>, IRemove<T>, IRemoveMany<T>, IGetById<T>, IGetMany<T>
    {
    }
}