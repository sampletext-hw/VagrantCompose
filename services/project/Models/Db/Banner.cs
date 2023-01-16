using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.Db.DbRestaurant;
using Models.Db.Relations;

namespace Models.Db
{
    public class Banner : IdEntity
    {
        public virtual ICollection<City> Cities { get; set; }
        
        public virtual ICollection<BannerToCity> CitiesRelation { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }
        
        [MaxLength(40)] // "c37af312-7862-412c-b9be-221cef70be5f.png"
        public string Image { get; set; }

        [MaxLength(4096)]
        public string ExtUrl { get; set; }
    }
}