using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.MobilePushes;

namespace Infrastructure.Implementations
{
    public class MobilePushByCityRepository : IdRepositoryBase<MobilePushByCity>, IMobilePushByCityRepository
    {
        public MobilePushByCityRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}