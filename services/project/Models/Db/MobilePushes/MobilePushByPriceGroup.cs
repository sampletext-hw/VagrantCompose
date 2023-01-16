using System.Collections.Generic;
using Models.Db.DbRestaurant;
using Models.Db.Relations;

namespace Models.Db.MobilePushes
{
    public class MobilePushByPriceGroup : MobilePushBase
    {
        public virtual ICollection<PriceGroup> PriceGroups { get; set; }

        public virtual ICollection<MobilePushToPriceGroup> PriceGroupsRelation { get; set; }
    }
}