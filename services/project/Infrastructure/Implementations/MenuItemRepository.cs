using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.Menu;

namespace Infrastructure.Implementations
{
    public class MenuItemRepository : IdRepositoryBase<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}