using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.Common;

namespace Infrastructure.Implementations
{
    public class CategoryRepository : IdRepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<Category> GetByTitle(string title)
        {
            return await GetDbSetT()
                .FirstOrDefaultAsync(pc => pc.Title == title);
        }

        public async Task<ICollection<Category>> GetAllOrdered()
        {
            return await GetDbSetT()
                .AsNoTracking()
                .OrderBy(c => c.Order)
                .ToListAsync();
        }
    }
}