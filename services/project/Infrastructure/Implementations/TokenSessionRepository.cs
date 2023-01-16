using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.Sessions;

namespace Infrastructure.Implementations
{
    public class TokenSessionRepository : IdRepositoryBase<TokenSession>, ITokenSessionRepository
    {
        public TokenSessionRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<TokenSession> GetByToken(string token)
        {
            return await GetDbSetT()
                .FirstOrDefaultAsync(ts => ts.Token == token);
        }
        
        public async Task<TokenSession> GetByTokenNonTracking(string token)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.Token == token);
        }

        public async Task<ICollection<TokenSession>> GetActiveByWorker(long workerId)
        {
            return await GetDbSetT()
                .Where(ts => ts.WorkerAccountId == workerId && ts.EndDate > DateTime.Now)
                .OrderBy(ts => ts.Id)
                .ToListAsync();
        }

        public async Task<ICollection<TokenSession>> GetActiveByWorkerNonTracking(long workerId)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .Where(ts => ts.WorkerAccountId == workerId && ts.EndDate > DateTime.Now)
                .OrderBy(ts => ts.Id)
                .ToListAsync();
        }
    }
}