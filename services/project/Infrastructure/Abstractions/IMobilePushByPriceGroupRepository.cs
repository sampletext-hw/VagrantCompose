using Infrastructure.BaseAbstractions;
using Models.Db.MobilePushes;

namespace Infrastructure.Abstractions
{
    using T = MobilePushByPriceGroup;
    public interface IMobilePushByPriceGroupRepository : IAdd<T>, IGetById<T>, IGetMany<T>, IRemove<T>
    {
    }
}