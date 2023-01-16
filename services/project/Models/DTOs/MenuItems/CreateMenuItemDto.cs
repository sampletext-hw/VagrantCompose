using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Common;
using Models.DTOs.General;
using Models.DTOs.MeasureDto;
using Models.DTOs.MenuItemToMenuProductDto;
using Models.DTOs.MenuItemToPriceGroupDto;
using Models.DTOs.Misc;

namespace Models.DTOs.MenuItems
{
    public class CreateMenuItemDto : IDto
    {
        [Required]
        [String(1, 48)]
        public string Title { get; set; }

        [Required]
        public CPFCDto CPFC { get; set; }

        [Required]
        [Id(typeof(Category))]
        public long CategoryId { get; set; }

        [Required]
        public ICollection<MenuItemToPriceGroupRelationDto> Prices { get; set; }

        [Required]
        public ICollection<MenuItemToMenuProductRelationDto> Products { get; set; }

        [Required]
        [String(1, 40)]
        public string Image { get; set; }

        [Required]
        public ICollection<MenuItemMeasureDto> Measures { get; set; }
    }
}