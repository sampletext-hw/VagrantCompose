using Models.DTOs.Misc;

namespace Models.DTOs.Carts
{
    public class MobileCartItemDto : IDto
    {
        public long MenuItemId { get; set; }

        public uint Amount { get; set; }
    }
}