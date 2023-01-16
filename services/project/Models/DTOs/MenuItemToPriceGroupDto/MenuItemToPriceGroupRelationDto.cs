using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;

namespace Models.DTOs.MenuItemToPriceGroupDto
{
    public class MenuItemToPriceGroupRelationDto : IDto
    {
        [Required]
        [Id(typeof(PriceGroup))]
        public long PriceGroupId { get; set; }
        
        [Required]
        [Range(0, 9999.0)]
        public float Price { get; set; }
    }
}