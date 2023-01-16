using Infrastructure.Abstractions;
using Infrastructure.BaseImplementations;
using Models.Db.DbOrder;

namespace Infrastructure.Implementations
{
    public class OrderItemRepository : IdRepositoryBase<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AkianaDbContext context) : base(context)
        {
        }
    }
}