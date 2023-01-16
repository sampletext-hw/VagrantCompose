using System;
using System.Collections.Generic;
using Models.Db.DbOrder;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Models.DTOs.Orders
{
    public class OrderMobileDto : IDto
    {
        public long Id { get; set; }

        public DateTime CreatedAtDateTime { get; set; }

        public ICollection<OrderItemMobileDto> OrderItems { get; set; }

        public float TotalCost { get; set; }
    }
}