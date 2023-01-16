using System.Threading.Tasks;

namespace Services.Shared.Abstractions
{
    public interface IIPCache
    {
        Task<(bool, string)> GetIsValidISP(string ip);
    }
}