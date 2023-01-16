using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IGetAllAsDictionary<T>
    {
        public Task<Dictionary<long, T>> GetAllAsDictionary();
    }
}