using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.Menu;

namespace Infrastructure.Implementations
{
    public class MenuProductRepository : IdRepositoryBase<MenuProduct>, IMenuProductRepository
    {
        public MenuProductRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}