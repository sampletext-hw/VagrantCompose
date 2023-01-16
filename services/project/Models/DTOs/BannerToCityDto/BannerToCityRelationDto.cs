using System.ComponentModel.DataAnnotations;
using Models.DTOs.Misc;

namespace Models.DTOs.BannerToCityDto
{
    public class BannerToCityRelationDto : IDto
    {
        [Required]
        public long CityId { get; set; }
    }
}