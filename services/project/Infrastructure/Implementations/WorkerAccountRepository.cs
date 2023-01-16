using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.Account;

namespace Infrastructure.Implementations
{
    public class WorkerAccountRepository : IdRepositoryBase<WorkerAccount>, IWorkerAccountRepository
    {
        public WorkerAccountRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}