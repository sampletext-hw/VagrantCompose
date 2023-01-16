using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;
using Models.Db.Relations;

namespace Models.Db.Menu
{
    public class MenuProduct : IdEntity
    {
        [MaxLength(48)]
        public string Title { get; set; }

        [MaxLength(512)]
        public string Content { get; set; }

        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<MenuItemToMenuProduct> MenuItemsRelation { get; set; }

        [MaxLength(40)]
        public string Image { get; set; }
    }
}