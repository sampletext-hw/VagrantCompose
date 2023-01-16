using Infrastructure.BaseAbstractions;
using Models.Db.Menu;

namespace Infrastructure.Abstractions
{
    public interface IMenuItemRepository : IGetById<MenuItem>, IAdd<MenuItem>, IUpdate<MenuItem>, IRemove<MenuItem>, IGetMany<MenuItem>, ICount<MenuItem>
    {
    }
}