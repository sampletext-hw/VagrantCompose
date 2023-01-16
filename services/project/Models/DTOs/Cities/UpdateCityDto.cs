using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Cities
{
    public class UpdateCityDto : IDto
    {
        [Required]
        [Id(typeof(City))]
        public long Id { get; set; }
        
        [Required]
        [Id(typeof(PriceGroup))]
        public long PriceGroupId { get; set; }
        
        [Required]
        [Range(-12, 12)]
        public int GmtOffsetFromMoscow { get; set; }
        
        [Required]
        [String(1, 64)]
        public string Title { get; set; }
        
        [Required]
        public CityLatLngDto LatLng { get; set; }
    }
}