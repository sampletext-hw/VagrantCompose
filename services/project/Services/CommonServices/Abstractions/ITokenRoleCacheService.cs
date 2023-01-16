using System.Threading.Tasks;

namespace Services.CommonServices.Abstractions
{
    public interface ITokenRoleCacheService
    {
        Task<bool> HasRole(string token, string role);

        Task<bool> HasAnyRole(string token, string[] roles);

        void Save(string token, string[] roles);

        Task Clear(string token);
        
        Task Clear(long workerId);
    }
}