using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IGetLastVersion<T>
    {
        Task<T> GetLastVersion();
        Task<T> GetLastVersionNonTracking();
    }
}