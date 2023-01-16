using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.DTOs.General;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Restaurants
{
    public class CreateRestaurantDto : IDto
    {
        [Required]
        [Id(typeof(City))]
        public long CityId { get; set; }

        [Required]
        [String(1, 64)]
        public string Title { get; set; }

        [Required]
        [String(1, 128)]
        public string Address { get; set; }

        [Required]
        public RestaurantLatLngDto Location { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 128)]
        public string SberbankUsername { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 128)]
        public string SberbankPassword { get; set; }

        [Required]
        public ICollection<PaymentTypeFlags> SupportedPaymentTypes { get; set; }
        
        [Required]
        public ICollection<DeliveryZoneLatLngDto> DeliveryZone { get; set; }

        [Required]
        public ICollection<OpenCloseTimeDto> PickupTimes { get; set; }

        [Required]
        public ICollection<OpenCloseTimeDto> DeliveryTimes { get; set; }
    }
}