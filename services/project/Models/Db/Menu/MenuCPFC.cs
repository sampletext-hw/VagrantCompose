using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Common;

namespace Models.Db.Menu
{
    public class MenuCPFC : IdEntity
    {
        [Range(0, 999.0)]
        public float Calories { get; set; }
        
        [Range(0, 999.0)]
        public float Proteins { get; set; }
        
        [Range(0, 999.0)]
        public float Fats { get; set; }
        
        [Range(0, 999.0)]
        public float Carbohydrates { get; set; }

        [ForeignKey(nameof(MenuItem))]
        public long MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }
    }
}