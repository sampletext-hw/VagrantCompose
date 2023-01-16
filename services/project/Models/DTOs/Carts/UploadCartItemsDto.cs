using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.Carts
{
    public class UploadCartItemsDto : IDto
    {
        public long ClientAccountId { get; set; }
        public ICollection<AddCartItemDto> CartItems { get; set; }
    }
}