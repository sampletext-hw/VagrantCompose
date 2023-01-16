using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.MobilePushes;

namespace Infrastructure.Implementations
{
    public class MobilePushByPriceGroupRepository : IdRepositoryBase<MobilePushByPriceGroup>, IMobilePushByPriceGroupRepository
    {
        public MobilePushByPriceGroupRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}