using Infrastructure.BaseAbstractions;
using Models.Db;

namespace Infrastructure.Abstractions
{
    using T = DeliveryAddress;
    
    public interface IDeliveryAddressRepository : IGetById<T>, IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, ICount<T>
    {
    }
}