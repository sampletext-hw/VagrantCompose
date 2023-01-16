using System;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Account;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;

namespace Models.DTOs.Orders
{
    public class MobileCreateOrderFromCartV2Dto : IDto
    {
        [Required]
        [Id(typeof(ClientAccount))]
        public long ClientAccountId { get; set; }

        [Required]
        [Id(typeof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        [EnumDataType(typeof(OrderPickupType))]
        public OrderPickupType PickupType { get; set; }

        [RequiredIf(nameof(PickupType), OrderPickupType.Delivery)]
        public long? DeliveryAddressId { get; set; }

        [Required]
        [EnumDataType(typeof(OrderDelayType))]
        public OrderDelayType DelayType { get; set; }

        [RequiredIf(nameof(DelayType), OrderDelayType.Delayed)]
        public DateTime? AwaitedAtDateTime { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentTypeFlags))]
        public PaymentTypeFlags PaymentType { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 256)]
        public string Comment { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 16)]
        public string Promocode { get; set; }

        [Required]
        public Guid UniqueId { get; set; }
    }
}