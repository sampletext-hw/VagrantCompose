using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Account;

namespace Infrastructure.Abstractions
{
    using T = WorkerAccount;
    public interface IWorkerAccountRepository  : IGetById<T>, IGetOne<T>, IAdd<T>, IUpdate<T>, IRemove<T>, ICount<T>, IGetMany<T>
    {
    }
}