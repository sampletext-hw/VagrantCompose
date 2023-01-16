using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db;
using Models.Db.DbRestaurant;

namespace Infrastructure.Implementations
{
    public class RestaurantRepository : IdRepositoryBase<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<bool> CanBeManagedBy(long id, long workerId)
        {
            return await GetDbSetT()
                .AnyAsync(r => r.Id == id && r.WorkerAccountsRelation.Any(rel => rel.WorkerAccountId == workerId));
        }
    }
}