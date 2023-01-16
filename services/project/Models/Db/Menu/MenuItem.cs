using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;
using Models.Db.DbCart;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.Relations;

namespace Models.Db.Menu
{
    public class MenuItem : IdEntity
    {
        [MaxLength(48)]
        public string Title { get; set; }

        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }

        // Price is inside relation
        public virtual ICollection<PriceGroup> PriceGroups { get; set; }
        public virtual ICollection<MenuItemToPriceGroup> PriceGroupsRelation { get; set; }

        public virtual ICollection<MenuProduct> MenuProducts { get; set; }
        public virtual ICollection<MenuItemToMenuProduct> MenuProductsRelation { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual ICollection<FavoriteItem> FavoriteItems { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        [MaxLength(40)]
        public string Image { get; set; }

        public virtual MenuCPFC CPFC { get; set; }

        public virtual ICollection<MenuItemMeasure> Measures { get; set; }
    }
}