using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;
using Models.Db.Menu;

namespace Models.Db.DbOrder
{
    public class OrderItem : IdEntity
    {
        [ForeignKey(nameof(Order))]
        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }
        
        public virtual MenuItem MenuItem { get; set; }

        public float PurchasePrice { get; set; }
    }
}