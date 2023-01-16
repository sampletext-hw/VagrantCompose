using Infrastructure.BaseAbstractions;
using Models.Db.Menu;

namespace Infrastructure.Abstractions
{
    using T = MenuProduct;

    public interface IMenuProductRepository : IGetById<T>, IAdd<T>, IUpdate<T>, IRemove<T>, IGetMany<T>, ICount<T>
    {
    }
}