using System.Collections.Generic;
using Models.Db.DbRestaurant;
using Models.Db.Relations;

namespace Models.Db.MobilePushes
{
    public class MobilePushByCity : MobilePushBase
    {
        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<MobilePushToCity> CitiesRelation { get; set; }
    }
}