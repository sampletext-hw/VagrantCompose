using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.Menu;

namespace Infrastructure.Implementations
{
    public class MenuCPFCRepository : IdRepositoryBase<MenuCPFC>, IMenuCPFCRepository
    {
        public MenuCPFCRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}