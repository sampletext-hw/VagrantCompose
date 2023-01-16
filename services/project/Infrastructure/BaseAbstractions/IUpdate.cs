using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IUpdate<in T>
    {
        Task Update(T entity);
    }
}