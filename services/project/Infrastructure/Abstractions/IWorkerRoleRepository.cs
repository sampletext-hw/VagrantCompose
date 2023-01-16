using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Account;

namespace Infrastructure.Abstractions
{
    using T = WorkerRole;
    public interface IWorkerRoleRepository  : IGetById<T>, IAdd<T>, IUpdate<T>, IRemove<T>, IGetMany<T>, ICount<T>
    {
        Task<T> GetByTitleEn(string titleEn);
    }
}