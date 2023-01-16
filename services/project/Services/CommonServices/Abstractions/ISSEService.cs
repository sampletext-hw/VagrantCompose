using System;

namespace Services.CommonServices.Abstractions
{
    public interface ISSEService
    {
        public event Action<long> OrderCreated;

        public long LastOrderId { set; get; }
        public void EmitOrderCreated(long id);
    }
}