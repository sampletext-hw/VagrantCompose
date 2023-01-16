using System.ComponentModel.DataAnnotations;
using Models.DTOs.Misc;

namespace Models.DTOs.LatLngDtos
{
    public class DeliveryZoneLatLngDto : IDto
    {
        [Range(-90, 90)]
        public float Lat { get; set; }
        
        [Range(-180, 180)]
        public float Lng { get; set; }
        public long Order { get; set; }

        public DeliveryZoneLatLngDto()
        {
        }

        public DeliveryZoneLatLngDto(float lat, float lng, long order)
        {
            Lat = lat;
            Lng = lng;
            Order = order;
        }
    }
}