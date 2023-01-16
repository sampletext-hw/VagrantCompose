using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.DbRestaurant;
using Models.Db.Menu;

namespace Models.Db.Relations
{
    public class MenuItemToPriceGroup
    {
        [ForeignKey(nameof(PriceGroup))]
        public long PriceGroupId { get; set; }

        public virtual PriceGroup PriceGroup { get; set; }
        
        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public float Price { get; set; }
    }
}