using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.DbOrder;
using Models.Db.Payments;

namespace Infrastructure.Implementations
{
    public class OrderRepository : IdRepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<Order> GetFullInfo(long id)
        {
            return await GetDbSetT()
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.MenuItem)
                .ThenInclude(i => i.Measures)
                .Include(o => o.Restaurant)
                .ThenInclude(r => r.City)
                .Include(o => o.ClientAccount)
                .Include(o => o.DeliveryAddress)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ICollection<Order>> GetByDateRangeForCallCenter(DateTime left, DateTime right)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(
                    o => o.CreatedAtDateTime >= left && o.CreatedAtDateTime < right &&
                         (o.PaymentType != PaymentTypeFlags.Online ||
                            (o.PaymentType == PaymentTypeFlags.Online && o.OnlinePayment.PaymentStatus == PaymentStatus.Payed)
                         )
                )
                .OrderByDescending(o => o.Id)
                .Include(o => o.ClientAccount)
                .Include(o => o.CreatorWorkerAccount)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                .Include(o => o.Restaurant)
                .ToListAsync();
        }

        public async Task<Order> GetByIdForMobile(long id)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.MenuItem)
                .Where(o => o.PaymentType != PaymentTypeFlags.Online ||
                            (o.PaymentType == PaymentTypeFlags.Online && o.OnlinePayment.PaymentStatus == PaymentStatus.Payed)
                )
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetByIdForCallCenter(long id)
        {
            return await GetDbSetT()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(o => o.ClientAccount)
                .Include(o => o.CreatorWorkerAccount)
                .Include(o => o.DeliveryAddress)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.MenuItem)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}