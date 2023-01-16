using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.LatLngs;

namespace Infrastructure.Implementations
{
    public class DeliveryZoneLatLngRepository : IdRepositoryBase<DeliveryZoneLatLng>, IDeliveryZoneLatLngRepository
    {
        public DeliveryZoneLatLngRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}