using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.DbOrder;
using Models.Db.LatLngs;

namespace Models.Db
{
    public class DeliveryAddress : IdEntity
    {
        [ForeignKey(nameof(ClientAccount))]
        public long ClientAccountId { get; set; }

        public virtual ClientAccount ClientAccount { get; set; }
        
        public virtual DeliveryAddressLatLng LatLng { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(64)]
        public string Floor { get; set; }

        [MaxLength(256)]
        public string Street { get; set; }

        [MaxLength(64)]
        public string Home { get; set; }

        [MaxLength(64)]
        public string Flat { get; set; }

        [MaxLength(64)]
        public string Entrance { get; set; }

        [MaxLength(256)]
        public string Comment { get; set; }
    }
}