using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.Db.Menu;
using Models.Db.MobilePushes;
using Models.Db.Relations;

namespace Models.Db.DbRestaurant
{
    public class PriceGroup : IdEntity
    {
        [MaxLength(32)]
        public string Title { get; set; }

        public virtual ICollection<City> Cities { get; set; }
        
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        
        public virtual ICollection<MenuItemToPriceGroup> PriceGroupItems { get; set; }
        
        public virtual ICollection<MobilePushByPriceGroup> MobilePushes { get; set; }
        
        public virtual ICollection<MobilePushToPriceGroup> MobilePushesRelation { get; set; }
    }
}