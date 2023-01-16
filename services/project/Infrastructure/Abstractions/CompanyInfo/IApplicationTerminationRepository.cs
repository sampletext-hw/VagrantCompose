using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    using T = ApplicationTerminationData;

    public interface IApplicationTerminationRepository : IAdd<T>, IRemove<T>, IUpdate<T>, IGetOne<T>, IGetLastVersion<T>
    {
    }
}