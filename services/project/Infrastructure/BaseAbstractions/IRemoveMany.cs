using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IRemoveMany<T>
    {
        Task RemoveMany(ICollection<T> entities);
    }
}