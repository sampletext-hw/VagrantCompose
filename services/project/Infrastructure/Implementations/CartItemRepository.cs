using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Microsoft.EntityFrameworkCore;
using Models.Db.DbCart;

namespace Infrastructure.Implementations
{
    public class CartItemRepository : IdRepositoryBase<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AkianaDbContext context) : base(context)
        {
        }

        public async Task<ICollection<CartItem>> GetCartItemsFromOnlinePaymentId(long onlinePaymentId)
        {
            var payments = GetContext().OnlinePayments;
            return await GetContext().CartItems
                .Where(c => payments
                    .Where(p => p.Id == onlinePaymentId)
                    .Select(p => p.Order.ClientAccountId)
                    .Contains(c.ClientAccountId)
                )
                .ToListAsync();
        }
    }
}