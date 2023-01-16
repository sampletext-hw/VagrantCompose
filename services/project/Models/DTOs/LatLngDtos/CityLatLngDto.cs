using System.ComponentModel.DataAnnotations;
using Models.DTOs.Misc;

namespace Models.DTOs.LatLngDtos
{
    public class CityLatLngDto : IDto
    {
        [Range(-90, 90)]
        public float Lat { get; set; }
        
        [Range(-180, 180)]
        public float Lng { get; set; }

        public CityLatLngDto()
        {
        }

        public CityLatLngDto(float lat, float lng)
        {
            Lat = lat;
            Lng = lng;
        }
    }
}