using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.PriceGroups
{
    public class CreatePriceGroupDto : IDto
    {
        [Required]
        [String(1, 32)]
        public string Title { get; set; }
    }
}