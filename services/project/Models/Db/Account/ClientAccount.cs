using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.Db.DbCart;
using Models.Db.DbOrder;

namespace Models.Db.Account
{
    public class ClientAccount : IdEntity
    {
        [MaxLength(12)]
        public string Login { get; set; }
        
        [MaxLength(32)]
        public string Username { get; set; }

        public DateTime? BirthDate { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<DeliveryAddress> DeliveryAddresses { get; set; }

        public virtual ICollection<ClientLoginRequest> LoginRequests { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual ICollection<FavoriteItem> FavoriteItems { get; set; }
    }
}