using Infrastructure.BaseAbstractions;
using Models.Db.LatLngs;

namespace Infrastructure.Abstractions
{
    using T = DeliveryZoneLatLng;

    public interface IDeliveryZoneLatLngRepository : IGetById<T>, IAdd<T>, IUpdate<T>, IRemove<T>, ICount<T>, IGetMany<T>, IRemoveMany<T>
    {
    }
}