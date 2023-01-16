using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.DbRestaurant;
using Models.Db.Payments;

namespace Models.Db.DbOrder
{
    public class Order : IdEntity
    {
        /// <summary>
        /// Internal title formed by
        /// <para>"CountInADay - ItemsCount"</para>
        /// </summary>
        [MaxLength(32)]
        public string Title { get; set; }

        [ForeignKey(nameof(CreatorWorkerAccount))]
        public long CreatorWorkerAccountId { get; set; }

        public virtual WorkerAccount CreatorWorkerAccount { get; set; }

        [ForeignKey(nameof(ClientAccount))]
        public long ClientAccountId { get; set; }

        public virtual ClientAccount ClientAccount { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        /// <summary>
        /// Server Date-Time when order was created. It's always stored without GMT offset from server
        /// </summary>
        public DateTime CreatedAtDateTime { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        // Pickup data

        public OrderPickupType PickupType { get; set; }

        public long? DeliveryAddressId { get; set; }

        public virtual DeliveryAddress DeliveryAddress { get; set; }

        public DateTime? DeliveredAtDateTime { get; set; }

        // Delay Data

        public OrderDelayType DelayType { get; set; }

        public DateTime AwaitedAtDateTime { get; set; }

        // Payment Data

        public PaymentTypeFlags PaymentType { get; set; }

        public virtual OnlinePayment OnlinePayment { get; set; }

        [MaxLength(256)]
        public string Comment { get; set; }

        [MaxLength(16)]
        public string Promocode { get; set; }

        // Уникальность заказа
        public Guid UniqueId { get; set; }

        public PostProcessingStatus PostProcessingStatus { get; set; }
    }
}