using System.Threading.Tasks;
using Models.Db.Account;
using Models.DTOs;
using Models.DTOs.ClientAccounts;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    public interface IClientAccountService
    {
        Task<ClientAccountDto> GetById(long id);
    }
}