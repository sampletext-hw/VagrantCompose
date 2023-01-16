using Infrastructure.BaseAbstractions;
using Models.Db;

namespace Infrastructure.Abstractions
{
    using T = ClientLoginRequest;

    public interface IClientLoginRequestRepository : IGetById<T>, IAdd<T>, IGetOne<T>, IRemove<T>, IUpdate<T>, ICount<T>
    {
    }
}