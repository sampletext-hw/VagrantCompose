using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions
{
    public interface IGetLast<T>
    {
        Task<T> GetLast(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
        
        Task<T> GetLastNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
    }
}