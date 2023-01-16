using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;
using Models.Db.DbRestaurant;

namespace Models.Db.Schedule
{
    public class OpenCloseTime : IdEntity
    {
        [Range(0, 7)]
        public uint DayOfWeek { get; set; }

        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }

        public bool IsWorking { get; set; }

        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}