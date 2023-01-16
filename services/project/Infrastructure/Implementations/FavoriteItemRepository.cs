using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.DbCart;

namespace Infrastructure.Implementations
{
    public class FavoriteItemRepository : IdRepositoryBase<FavoriteItem>, IFavoriteItemRepository
    {
        public FavoriteItemRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}