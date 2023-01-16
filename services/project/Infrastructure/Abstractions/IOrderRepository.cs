using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.DbOrder;

namespace Infrastructure.Abstractions
{
    using T = Order;
    public interface IOrderRepository : IGetById<T>, IAdd<T>, IUpdate<T>, IRemove<T>, ICount<T>, IGetMany<T>, IRemoveMany<T>, IGetOne<T>, ISaveChanges
    {
        /// <summary>
        /// Gets full info about order and should only be used in email sending
        /// </summary>
        Task<T> GetFullInfo(long id);

        Task<ICollection<T>> GetByDateRangeForCallCenter(DateTime left, DateTime right);

        Task<Order> GetByIdForMobile(long id);
        Task<Order> GetByIdForCallCenter(long id);
    }
}