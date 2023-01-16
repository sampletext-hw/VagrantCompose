using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.DbOrder;
using Models.DTOs.General;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Restaurants
{
    public class RestaurantWithIdDto : IDto
    {
        public long Id { get; set; }
        public string Address { get; set; }

        public string Title { get; set; }

        public RestaurantLatLngDto Location { get; set; }
        
        public ICollection<DeliveryZoneLatLngDto> DeliveryZone { get; set; }

        public long CityId { get; set; }
        public string CityTitle { get; set; }

        public bool IsWorking { get; set; }
        public bool IsPickupWorking { get; set; }
        public bool IsDeliveryWorking { get; set; }

        public ICollection<PaymentTypeFlags> SupportedPaymentTypes { get; set; }
        
        public string SberbankUsername { get; set; }

        public string SberbankPassword { get; set; }

        public ICollection<OpenCloseTimeDto> PickupTimes { get; set; }
        public ICollection<OpenCloseTimeDto> DeliveryTimes { get; set; }
    }
}