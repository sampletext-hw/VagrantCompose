using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Db.Common;

namespace Infrastructure.BaseImplementations
{
    public class VersionedRepositoryBase<T> : IdRepositoryBase<T> where T : VersionedEntity
    {
        protected VersionedRepositoryBase(AkianaDbContext context) : base(context)
        {
        }

        public async Task<T> GetLastVersion()
        {
            return await GetDbSetT()
                .OrderByDescending(t => t.Version)
                .FirstOrDefaultAsync();
        }
        
        public async Task<T> GetLastVersionNonTracking()
        {
            return await GetDbSetT()
                .AsNoTracking()
                .OrderByDescending(t => t.Version)
                .FirstOrDefaultAsync();
        }
    }
}