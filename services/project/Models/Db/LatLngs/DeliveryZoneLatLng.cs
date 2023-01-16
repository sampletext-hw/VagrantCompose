using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;

namespace Models.Db.LatLngs
{
    public class DeliveryZoneLatLng : LatLngBase
    {
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public long Order { get; set; }
    }
}