using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Account;
using Models.Db.Menu;
using Models.DTOs.Misc;

namespace Models.DTOs.Favorite
{
    public class AddFavoriteItemDto : IDto
    {
        [Required]
        [Id(typeof(ClientAccount))]
        public long ClientAccountId { get; set; }

        [Required]
        [Id(typeof(MenuItem))]
        public long MenuItemId { get; set; }
    }
}