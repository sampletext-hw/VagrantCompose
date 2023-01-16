using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;

namespace Models.Db.LatLngs
{
    public class CityLatLng : LatLngBase
    {
        [ForeignKey(nameof(City))]
        public long CityId { get; set; }

        public virtual City City { get; set; }
    }
}