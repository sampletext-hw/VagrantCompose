using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    using T = DeliveryTermsData;

    public interface IDeliveryTermsDataRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetOne<T>, IGetLastVersion<T>
    {
    }
}