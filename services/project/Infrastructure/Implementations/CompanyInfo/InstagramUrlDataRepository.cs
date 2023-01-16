using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class InstagramUrlDataRepository : VersionedRepositoryBase<InstagramUrlData>, IInstagramUrlDataRepository
    {
        public InstagramUrlDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}