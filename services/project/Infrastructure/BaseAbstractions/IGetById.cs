using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models.Db.Common;

namespace Infrastructure.BaseAbstractions
{
    public interface IGetById<T> where T : IdEntity
    {
        Task<T> GetById(long id, params Expression<Func<T, object>>[] includes);
        
        Task<T> GetByIdNonTracking(long id, params Expression<Func<T, object>>[] includes);
    }
}