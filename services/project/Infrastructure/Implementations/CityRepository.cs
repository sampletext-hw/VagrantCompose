using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.DbRestaurant;

namespace Infrastructure.Implementations
{
    public class CityRepository : IdRepositoryBase<City>, ICityRepository
    {
        public CityRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<int> GetGmtOffsetByRestaurant(long restaurantId)
        {
            return await Context.Restaurants
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(r => r.Id == restaurantId)
                .Select(r => r.City.GmtOffsetFromMoscow)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetGmtOffsetByOrder(long orderId)
        {
            return await Context.Orders
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(r => r.Id == orderId)
                .Select(o => o.Restaurant.City.GmtOffsetFromMoscow)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<long, int>> GetGmtOffsetsByOrders(ICollection<long> orderIds)
        {
            return await Context
                .Orders
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(r => orderIds.Contains(r.Id))
                .Select(o => new {o.Id, o.Restaurant.City.GmtOffsetFromMoscow})
                .ToDictionaryAsync(arg => arg.Id, arg => arg.GmtOffsetFromMoscow);
        }
    }
}