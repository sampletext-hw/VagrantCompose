using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Menu;
using Models.DTOs.Misc;

namespace Models.DTOs.MenuItemToMenuProductDto
{
    public class MenuItemToMenuProductRelationDto : IDto
    {
        [Required]
        [Id(typeof(MenuProduct))]
        public long MenuProductId { get; set; }
    }
}