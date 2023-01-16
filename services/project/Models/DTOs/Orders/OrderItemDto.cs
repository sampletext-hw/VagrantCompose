using Models.DTOs.Misc;

namespace Models.DTOs.Orders
{
    public class OrderItemDto : IDto
    {
        public long MenuItemId { get; set; }

        public float PurchasePrice { get; set; }
    }
}