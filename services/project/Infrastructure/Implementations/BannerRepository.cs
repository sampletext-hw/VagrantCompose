using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db;

namespace Infrastructure.Implementations
{
    public class BannerRepository : IdRepositoryBase<Banner>, IBannerRepository
    {
        public BannerRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}