using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BaseImplementations
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly AkianaDbContext Context;

        protected RepositoryBase(AkianaDbContext context)
        {
            Context = context;
        }

        protected DbSet<T> GetDbSetT() => Context.Set<T>();
        protected AkianaDbContext GetContext() => Context;

        public async Task<long> Count(Expression<Func<T, bool>> predicate = null)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .LongCountAsync();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync();
        }
        
        public async Task<T> GetOneNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await Context.SaveChangesAsync();
        }
    }
}