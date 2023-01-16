using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    using T = VkUrlData;

    public interface IVkUrlDataRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetOne<T>, IGetLastVersion<T>
    {
    }
}