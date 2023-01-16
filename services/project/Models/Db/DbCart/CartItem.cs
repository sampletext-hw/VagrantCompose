using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.Menu;

namespace Models.Db.DbCart
{
    public class CartItem : IdEntity
    {
        [ForeignKey(nameof(ClientAccount))]
        public long ClientAccountId { get; set; }

        public virtual ClientAccount ClientAccount { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }
        
        public virtual MenuItem MenuItem { get; set; }

        public uint Amount { get; set; }
    }
}