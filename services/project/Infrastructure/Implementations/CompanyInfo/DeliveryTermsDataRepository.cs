using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class DeliveryTermsDataRepository : VersionedRepositoryBase<DeliveryTermsData>, IDeliveryTermsDataRepository
    {
        public DeliveryTermsDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}