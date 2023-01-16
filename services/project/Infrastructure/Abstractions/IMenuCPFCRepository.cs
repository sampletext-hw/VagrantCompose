using Infrastructure.BaseAbstractions;
using Models.Db.Menu;

namespace Infrastructure.Abstractions
{
    using T = MenuCPFC;

    public interface IMenuCPFCRepository : IGetById<T>, IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, IGetOne<T>, ICount<T>
    {
    }
}