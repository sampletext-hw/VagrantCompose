using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface ICount<T>
    {
        Task<long> Count(Expression<Func<T, bool>> predicate = null);
    }
}