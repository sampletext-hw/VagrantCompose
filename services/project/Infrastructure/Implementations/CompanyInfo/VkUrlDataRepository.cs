using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class VkUrlDataRepository : VersionedRepositoryBase<VkUrlData>, IVkUrlDataRepository
    {
        public VkUrlDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}