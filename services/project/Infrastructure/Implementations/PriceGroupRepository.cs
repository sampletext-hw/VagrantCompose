using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db;
using Models.Db.DbRestaurant;

namespace Infrastructure.Implementations
{
    public class PriceGroupRepository : IdRepositoryBase<PriceGroup>, IPriceGroupRepository
    {
        public PriceGroupRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}