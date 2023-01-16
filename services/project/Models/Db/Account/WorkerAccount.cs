using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.Payments;
using Models.Db.Relations;
using Models.Db.RestaurantStop;
using Models.Db.Sessions;

namespace Models.Db.Account
{
    public class WorkerAccount : IdEntity
    {
        [MaxLength(32)]
        public string Login { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Surname { get; set; }

        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        [MaxLength(64)]
        public string Email { get; set; }

        [MaxLength(32)]
        public string Password { get; set; }

        public bool IsTechnical { get; set; }

        public virtual ICollection<WorkerRole> WorkerRoles { get; set; }
        public virtual ICollection<WorkerAccountToRole> WorkerRolesRelation { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
        public virtual ICollection<WorkerAccountToRestaurant> RestaurantsRelation { get; set; }

        public virtual ICollection<TokenSession> TokenSessions { get; set; }

        public virtual ICollection<Order> CreatedOrders { get; set; }

        public virtual ICollection<RestaurantPickupStop> IssuedPickupStops { get; set; }
        public virtual ICollection<RestaurantDeliveryStop> IssuedDeliveryStops { get; set; }

        public virtual ICollection<OnlinePayment> IssuedPayments { get; set; }
    }
}