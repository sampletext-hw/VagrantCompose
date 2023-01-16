using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    using T = InstagramUrlData;

    public interface IInstagramUrlDataRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetOne<T>, IGetLastVersion<T>
    {
    }
}