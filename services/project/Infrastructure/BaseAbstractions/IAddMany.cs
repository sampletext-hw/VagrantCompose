using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IAddMany<T>
    {
        Task AddMany(ICollection<T> entities);
    }
}