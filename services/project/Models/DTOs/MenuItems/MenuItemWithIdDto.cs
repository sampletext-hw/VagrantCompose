using System.Collections.Generic;
using Models.DTOs.General;
using Models.DTOs.MeasureDto;
using Models.DTOs.MenuItemToMenuProductDto;
using Models.DTOs.MenuItemToPriceGroupDto;
using Models.DTOs.Misc;

namespace Models.DTOs.MenuItems
{
    public class MenuItemWithIdDto : IDto
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public string Title { get; set; }
        
        public CPFCDto CPFC { get; set; }
        
        public ICollection<MenuItemToPriceGroupRelationDto> Prices { get; set; }
        
        public ICollection<MenuItemToMenuProductRelationDto> Products { get; set; }

        public string Image { get; set; }

        public ICollection<MenuItemMeasureDto> Measures { get; set; }
    }
}