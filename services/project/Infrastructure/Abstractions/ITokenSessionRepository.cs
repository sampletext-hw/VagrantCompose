using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Sessions;

namespace Infrastructure.Abstractions
{
    using T = TokenSession;
    public interface ITokenSessionRepository  : IGetById<T>, IAdd<T>, IUpdate<T>, IRemove<T>, ICount<T>
    {
        Task<T> GetByToken(string token);
        Task<T> GetByTokenNonTracking(string token);
        
        Task<ICollection<T>> GetActiveByWorker(long workerId);
        Task<ICollection<T>> GetActiveByWorkerNonTracking(long workerId);
    }
}