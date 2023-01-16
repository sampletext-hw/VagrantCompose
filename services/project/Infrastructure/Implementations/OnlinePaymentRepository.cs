using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.Payments;

namespace Infrastructure.Implementations;

public class OnlinePaymentRepository : IdRepositoryBase<OnlinePayment>, IOnlinePaymentRepository
{
    public OnlinePaymentRepository(AkianaDbContext context) : base(context)
    {
    }

    public async Task<OnlinePayment> GetByOrder(long orderId)
    {
        return await GetDbSetT()
            .Include(p => p.Order)
            .ThenInclude(o => o.ClientAccount)
            .FirstOrDefaultAsync(p => p.OrderId == orderId);
    }
}