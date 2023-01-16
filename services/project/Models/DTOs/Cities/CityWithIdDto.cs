using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Cities
{
    public class CityWithIdDto : IDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        
        public long PriceGroupId { get; set; }
        public string PriceGroupTitle { get; set; }
        
        public int GmtOffsetFromMoscow { get; set; }
        
        public CityLatLngDto LatLng { get; set; }
    }
}