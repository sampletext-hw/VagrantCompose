using System.Threading.Tasks;
using Models.Db.Account;
using Models.Db.Sessions;
using Models.DTOs.Misc;
using Models.DTOs.Requests;

namespace Services.Shared.Abstractions
{
    public interface ITokenSessionService
    {
        Task<LoginResultDto> Login(LoginDto loginDto, string ip);

        Task<TokenSession> GetByToken(string token);
        
        Task<long> GetAccountIdByToken(string token);
        
        Task Logout(string token);
        Task<LoginResultDto> LoginV2(LoginDto loginDto, string ip);
    }
}