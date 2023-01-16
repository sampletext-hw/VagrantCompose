using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.Account;

namespace Infrastructure.Implementations
{
    public class ClientAccountRepository : IdRepositoryBase<ClientAccount>, IClientAccountRepository
    {
        public ClientAccountRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}