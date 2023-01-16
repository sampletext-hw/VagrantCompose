using System;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations
{
    public class SSEService : ISSEService
    {
        public event Action<long> OrderCreated;
        public long LastOrderId { get; set; }

        public void EmitOrderCreated(long id)
        {
            OrderCreated?.Invoke(id);
        }
    }
}