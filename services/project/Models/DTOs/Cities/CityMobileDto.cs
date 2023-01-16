using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Cities
{
    public class CityMobileDto : IDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        public long PriceGroupId { get; set; }
        
        public CityLatLngDto LatLng { get; set; }
    }
}