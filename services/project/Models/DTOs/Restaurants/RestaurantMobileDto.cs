using System.Collections.Generic;
using Models.Db.DbOrder;
using Models.Db.RestaurantStop;
using Models.DTOs.General;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;

namespace Models.DTOs.Restaurants
{
    public class RestaurantMobileDto : IDto
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public RestaurantLatLngDto Location { get; set; }
        
        public ICollection<DeliveryZoneLatLngDto> DeliveryZone { get; set; }

        public long CityId { get; set; }

        public bool IsWorking { get; set; }
        public bool IsPickupWorking { get; set; }
        public bool IsDeliveryWorking { get; set; }

        public ICollection<PaymentTypeFlags> SupportedPaymentTypes { get; set; }
        
        public RestaurantStopReason LastPickupStopReason { get; set; }
        public RestaurantStopReason LastDeliveryStopReason { get; set; }

        public ICollection<OpenCloseTimeDto> PickupTimes { get; set; }
        public ICollection<OpenCloseTimeDto> DeliveryTimes { get; set; }
    }
}