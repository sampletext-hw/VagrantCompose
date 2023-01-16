using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Payments;

namespace Infrastructure.Abstractions;

public interface IOnlinePaymentRepository : IAdd<OnlinePayment>, IUpdate<OnlinePayment>, IGetOne<OnlinePayment>, ISaveChanges
{
    Task<OnlinePayment> GetByOrder(long orderId);
}