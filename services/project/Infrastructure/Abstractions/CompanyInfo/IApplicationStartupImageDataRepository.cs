using Infrastructure.BaseAbstractions;
using Models.Db.CompanyInfo;

namespace Infrastructure.Abstractions.CompanyInfo
{
    public interface IApplicationStartupImageDataRepository : IAdd<ApplicationStartupImageData>, IRemove<ApplicationStartupImageData>, IUpdate<ApplicationStartupImageData>, IGetOne<ApplicationStartupImageData>, IGetLastVersion<ApplicationStartupImageData>
    {
        
    }
}