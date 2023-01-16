using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.DeliveryAddresses
{
    public class AddDeliveryAddressDto : IDto
    {
        [Required]
        [Id(typeof(ClientAccount))]
        public long ClientAccountId { get; set; }

        public DeliveryAddressLatLngDto LatLng { get; set; }

        [Required]
        [String(1, 256)]
        public string Street { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 64)]
        public string Title { get; set; }

        [Required]
        [String(1, 16)]
        public string Home { get; set; }
        
        [Required(AllowEmptyStrings = true)]
        [String(0, 16)]
        public string Floor { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 16)]
        public string Flat { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 16)]
        public string Entrance { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 256)]
        public string Comment { get; set; }
    }
}