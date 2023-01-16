using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;
using Models.Db.MobilePushes;

namespace Models.Db.Relations
{
    public class MobilePushToPriceGroup
    {
        [ForeignKey(nameof(MobilePushByPriceGroup))]
        public long MobilePushByPriceGroupId { get; set; }

        public virtual MobilePushByPriceGroup MobilePushByPriceGroup { get; set; }

        [ForeignKey(nameof(PriceGroup))]
        public long PriceGroupId { get; set; }

        public virtual PriceGroup PriceGroup { get; set; }
    }
}