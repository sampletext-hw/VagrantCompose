using Infrastructure.BaseAbstractions;
using Models.Db.MobilePushes;

namespace Infrastructure.Abstractions
{
    using T = MobilePushByCity;
    public interface IMobilePushByCityRepository : IAdd<T>, IGetById<T>, IGetMany<T>, IRemove<T>
    {
    }
}