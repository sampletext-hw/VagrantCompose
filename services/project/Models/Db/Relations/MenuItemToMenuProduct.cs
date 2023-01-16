using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Menu;

namespace Models.Db.Relations
{
    public class MenuItemToMenuProduct
    {
        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        [ForeignKey(nameof(MenuProduct))]
        public long MenuProductId { get; set; }

        public virtual MenuProduct MenuProduct { get; set; }
    }
}