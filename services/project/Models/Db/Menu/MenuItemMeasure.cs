using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;

namespace Models.Db.Menu
{
    public class MenuItemMeasure : IdEntity
    {
        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public MeasureType MeasureType { get; set; }

        [Range(0, 9999.0)]
        public float Amount { get; set; }
    }
}