using System;
using Models.Db.RestaurantStop;
using Models.DTOs.Misc;

namespace Models.DTOs.RestaurantStops
{
    public class RestaurantStopDto : IDto
    {
        public long RestaurantId { get; set; }

        public long IssuerId { get; set; }

        public RestaurantStopReason Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}