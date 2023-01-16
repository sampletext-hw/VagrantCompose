using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.BaseImplementations;
using Models.Db.CompanyInfo;

namespace Infrastructure.Implementations.CompanyInfo
{
    public class VacanciesDataRepository : VersionedRepositoryBase<VacanciesData>, IVacanciesDataRepository
    {
        public VacanciesDataRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}