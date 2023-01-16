using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;

namespace Models.Db.Relations
{
    public class BannerToCity
    {
        [ForeignKey(nameof(Banner))]
        public long BannerId { get; set; }

        public virtual Banner Banner { get; set; }

        [ForeignKey(nameof(City))]
        public long CityId { get; set; }

        public virtual City City { get; set; }
    }
}