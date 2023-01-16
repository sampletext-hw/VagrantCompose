using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.Db.RestaurantStop;
using Models.DTOs.Misc;

namespace Models.DTOs.RestaurantStops
{
    public class CreateRestaurantStopDto : IDto
    {
        [Required]
        [Id(typeof(Restaurant))]
        public long RestaurantId { get; set; }

        [Required]
        [EnumDataType(typeof(RestaurantStopReason))]
        public RestaurantStopReason Reason { get; set; }
    }
}