using System;
using System.Collections.Generic;
using Models.Db.DbOrder;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Models.DTOs.Orders
{
    public class OrderDto : IDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public long ClientAccountId { get; set; }

        public string ClientAccountLogin { get; set; }

        public string ClientAccountUsername { get; set; }

        public long CreatorWorkerAccountId { get; set; }

        public string CreatorWorkerAccountName { get; set; }
        
        public string CreatorWorkerAccountSurname { get; set; }

        public DateTime CreatedAtDateTime { get; set; }

        public DateTime AwaitedAtDateTime { get; set; }

        public long RestaurantId { get; set; }

        public string RestaurantTitle { get; set; }

        public string RestaurantAddress { get; set; }

        public ICollection<OrderItemDto> OrderItems { get; set; }

        // Pickup data

        public OrderPickupType PickupType { get; set; }

        public long? DeliveryAddressId { get; set; }

        public DeliveryAddressDto DeliveryAddress { get; set; }

        public DateTime? DeliveredAtDateTime { get; set; }

        // Delay data

        public OrderDelayType DelayType { get; set; }

        public PaymentTypeFlags PaymentType { get; set; }

        public string Comment { get; set; }

        public float TotalCost { get; set; }

        public string Promocode { get; set; }
    }
}