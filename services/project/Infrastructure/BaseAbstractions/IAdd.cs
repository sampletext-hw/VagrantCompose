using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IAdd<T>
    {
        Task Add(T entity);
    }
}