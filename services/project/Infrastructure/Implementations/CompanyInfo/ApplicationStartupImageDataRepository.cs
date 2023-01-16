using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class ApplicationStartupImageDataRepository : VersionedRepositoryBase<ApplicationStartupImageData>, IApplicationStartupImageDataRepository
    {
        public ApplicationStartupImageDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}