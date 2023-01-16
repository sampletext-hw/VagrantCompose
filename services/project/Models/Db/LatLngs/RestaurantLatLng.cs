using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;

namespace Models.Db.LatLngs
{
    public class RestaurantLatLng : LatLngBase
    {
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}