using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.DeliveryAddresses
{
    public class DeliveryAddressDto : IDto
    {
        public long Id { get; set; }
        
        public long ClientAccountId { get; set; }

        public DeliveryAddressLatLngDto LatLng { get; set; }

        public string Title { get; set; }
        
        public string Floor { get; set; }
        
        public string Street { get; set; }

        public string Home { get; set; }

        public string Flat { get; set; }

        public string Entrance { get; set; }

        public string Comment { get; set; }
    }
}