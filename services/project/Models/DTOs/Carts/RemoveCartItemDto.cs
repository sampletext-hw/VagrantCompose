using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Account;
using Models.Db.Menu;
using Models.DTOs.Misc;

namespace Models.DTOs.Carts
{
    public class RemoveCartItemDto : IDto
    {
        [Required]
        [Id(typeof(ClientAccount))]
        public long ClientAccountId { get; set; }
        
        [Required]
        [Id(typeof(MenuItem))]
        public long MenuItemId { get; set; }

        public uint Amount { get; set; }
    }
}