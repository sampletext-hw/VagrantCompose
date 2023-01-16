using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class ApplicationTerminationRepository : VersionedRepositoryBase<ApplicationTerminationData>, IApplicationTerminationRepository
    {
        public ApplicationTerminationRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}