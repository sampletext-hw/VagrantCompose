using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.Account;

namespace Infrastructure.Implementations
{
    public class WorkerRoleRepository : IdRepositoryBase<WorkerRole>, IWorkerRoleRepository
    {
        public WorkerRoleRepository(AkianaDbContext context) : base(context)
        {
        }
        
        public async Task<WorkerRole> GetByTitleEn(string titleEn)
        {
            return await GetDbSetT()
                .FirstOrDefaultAsync(wr => wr.TitleEn == titleEn);
        }
    }
}