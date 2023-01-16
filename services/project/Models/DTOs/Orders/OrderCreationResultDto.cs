using Models.DTOs.Misc;

namespace Models.DTOs.Orders;

public class OrderCreationResultDto : IDto
{
    public long OrderId { get; set; }

    public string PaymentUrl { get; set; }
}