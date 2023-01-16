using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Menu;

namespace Models.Db.Common
{
    public class Category : IdEntity
    {
        [MaxLength(32)]
        public string Title { get; set; }

        public int Order { get; set; }

        public virtual ICollection<MenuProduct> MenuProducts { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        
        public bool IsDeletable { get; set; }
    }
}