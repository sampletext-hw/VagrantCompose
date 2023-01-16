using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BaseImplementations
{
    public class NoIdRepositoryBase<T> : RepositoryBase<T> where T : class
    {
        protected NoIdRepositoryBase(AkianaDbContext context) : base(context)
        {
        }

        public async Task Update(T entity)
        {
            GetDbSetT().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {
            GetDbSetT().Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Add(T entity)
        {
            GetDbSetT().Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .AggregateIncludes(includes)
                .ToListAsync();
        }
    }
}