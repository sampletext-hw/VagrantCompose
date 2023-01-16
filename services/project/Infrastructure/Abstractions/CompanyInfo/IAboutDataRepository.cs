using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    using T = AboutData;

    public interface IAboutDataRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetOne<T>, IGetLastVersion<T>
    {
    }
}