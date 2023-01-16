using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;
using Models.Db.MobilePushes;

namespace Models.Db.Relations
{
    public class MobilePushToCity
    {
        [ForeignKey(nameof(MobilePushByCity))]
        public long MobilePushByCityId { get; set; }

        public virtual MobilePushByCity MobilePushByCity { get; set; }

        [ForeignKey(nameof(City))]
        public long CityId { get; set; }

        public virtual City City { get; set; }
    }
}