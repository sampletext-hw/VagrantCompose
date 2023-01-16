using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Account;

namespace Infrastructure.Abstractions
{
    using T = ClientAccount;

    public interface IClientAccountRepository : IGetById<T>, IGetOne<T>, IAdd<T>, IUpdate<T>, IRemove<T>, ICount<T>
    {
    }
}