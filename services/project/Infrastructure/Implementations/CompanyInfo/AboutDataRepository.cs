using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class AboutDataRepository : VersionedRepositoryBase<AboutData>, IAboutDataRepository
    {
        public AboutDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}