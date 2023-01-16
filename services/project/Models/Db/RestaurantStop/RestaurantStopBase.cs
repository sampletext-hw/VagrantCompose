using System;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.DbRestaurant;

namespace Models.Db.RestaurantStop
{
    public class RestaurantStopBase : IdEntity
    {
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(Issuer))]
        public long IssuerId { get; set; }

        public virtual WorkerAccount Issuer { get; set; }

        public RestaurantStopReason Reason { get; set; }

        public DateTime StartDate { get; set; }

        // Date when it's closed
        public DateTime? EndDate { get; set; }
    }
}