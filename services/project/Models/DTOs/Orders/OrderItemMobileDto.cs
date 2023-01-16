using Models.DTOs.MenuItems;
using Models.DTOs.Misc;

namespace Models.DTOs.Orders
{
    public class OrderItemMobileDto : IDto
    {
        public MenuItemMobileDto MenuItem { get; set; }
        
        public float PurchasePrice { get; set; }
    }
}