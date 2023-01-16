using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;
using Models.Db.LatLngs;
using Models.Db.MobilePushes;
using Models.Db.Relations;

namespace Models.Db.DbRestaurant
{
    public class City : IdEntity
    {
        [MaxLength(64)]
        public string Title { get; set; }

        [ForeignKey(nameof(PriceGroup))]
        public long PriceGroupId { get; set; }

        public virtual PriceGroup PriceGroup { get; set; }

        public int GmtOffsetFromMoscow { get; set; }
        
        public virtual CityLatLng LatLng { get; set; }
        
        public virtual ICollection<Banner> Banners { get; set; }
        
        public virtual ICollection<BannerToCity> BannersRelation { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
        
        public virtual ICollection<MobilePushByCity> MobilePushes { get; set; }
        
        public virtual ICollection<MobilePushToCity> MobilePushesRelation { get; set; }
    }
}