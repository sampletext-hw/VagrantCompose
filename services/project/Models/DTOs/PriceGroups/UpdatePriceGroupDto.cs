using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;

namespace Models.DTOs.PriceGroups
{
    public class UpdatePriceGroupDto : IDto
    {
        [Required]
        [Id(typeof(PriceGroup))]
        public long Id { get; set; }
        
        [Required]
        [String(1, 32)]
        public string Title { get; set; }
    }
}