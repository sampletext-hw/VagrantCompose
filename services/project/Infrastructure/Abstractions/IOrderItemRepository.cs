using Infrastructure.BaseAbstractions;
using Models.Db.DbOrder;

namespace Infrastructure.Abstractions
{
    public interface IOrderItemRepository : IGetById<OrderItem>, IAdd<OrderItem>, IUpdate<OrderItem>, IRemove<OrderItem>, IGetMany<OrderItem>, ICount<OrderItem>
    {
    }
}