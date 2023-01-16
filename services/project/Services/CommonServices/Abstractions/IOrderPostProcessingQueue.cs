using System.Threading;
using System.Threading.Tasks;
using Models.Internals;

namespace Services.CommonServices.Abstractions;

public interface IOrderPostProcessingQueue
{
    Task Enqueue(long creatorId, long orderId);

    Task<OrderPostProcessItem> Dequeue(CancellationToken cancellationToken);
}