using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db;

namespace Infrastructure.Implementations
{
    public class ClientLoginRequestRepository : IdRepositoryBase<ClientLoginRequest>, IClientLoginRequestRepository
    {
        public ClientLoginRequestRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}