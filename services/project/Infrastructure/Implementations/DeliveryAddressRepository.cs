using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db;

namespace Infrastructure.Implementations
{
    public class DeliveryAddressRepository : IdRepositoryBase<DeliveryAddress>, IDeliveryAddressRepository
    {
        public DeliveryAddressRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}