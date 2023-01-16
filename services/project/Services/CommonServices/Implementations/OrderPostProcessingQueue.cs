using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Models.Internals;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations;

public class OrderPostProcessingQueue : IOrderPostProcessingQueue
{
    private readonly Channel<OrderPostProcessItem> _queue;

    public OrderPostProcessingQueue()
    {
        var options = new BoundedChannelOptions(3)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<OrderPostProcessItem>(options);
    }

    public async Task Enqueue(long creatorId, long orderId)
    {
        await _queue.Writer.WriteAsync(new OrderPostProcessItem(creatorId, orderId));
    }

    public async Task<OrderPostProcessItem> Dequeue(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}