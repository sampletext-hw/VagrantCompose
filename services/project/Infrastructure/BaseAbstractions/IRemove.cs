using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IRemove<T>
    {
        Task Remove(T entity);
    }
}