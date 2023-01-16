using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Db.Common;

namespace Infrastructure.BaseImplementations
{
    public class IdRepositoryBase<T> : RepositoryBase<T> where T : IdEntity
    {
        protected IdRepositoryBase(AkianaDbContext context) : base(context)
        {
        }

        public async Task Update(T entity)
        {
            GetDbSetT().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(T entity)
        {
            entity.IsSoftDeleted = true;
            GetDbSetT().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveMany(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsSoftDeleted = true;
            }

            GetDbSetT().UpdateRange(entities);
            await Context.SaveChangesAsync();
        }

        public async Task Add(T entity)
        {
            GetDbSetT().Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task AddMany(ICollection<T> entities)
        {
            GetDbSetT().AddRange(entities);
            await Context.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .OrderBy(e => e.Id)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .OrderBy(e => e.Id)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetMany(Expression<Func<T, bool>> predicate = null, int offset = 0, int limit = 25, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .OrderBy(e => e.Id)
                .Skip(offset).Take(limit)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyNonTracking(Expression<Func<T, bool>> predicate = null, int offset = 0, int limit = 25, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .OrderBy(e => e.Id)
                .Skip(offset).Take(limit)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyReversed(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyReversedNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyReversed(Expression<Func<T, bool>> predicate = null, int offset = 0, int limit = 25, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .Skip(offset).Take(limit)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<ICollection<T>> GetManyReversedNonTracking(Expression<Func<T, bool>> predicate = null, int offset = 0, int limit = 25, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .Skip(offset).Take(limit)
                .AggregateIncludes(includes)
                .ToListAsync();
        }

        public async Task<T> GetById(long id, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        
        public async Task<T> GetByIdNonTracking(long id, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<T> GetLast(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetLastNonTracking(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .ApplyPredicate(predicate)
                .OrderByDescending(e => e.Id)
                .AggregateIncludes(includes)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<long, T>> GetAllAsDictionary()
        {
            return
                (await GetDbSetT().ToListAsync())
                .ToDictionary(entity => entity.Id, entity => entity);
        }
    }
}