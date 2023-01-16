using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.DbOrder;
using Models.Db.LatLngs;
using Models.Db.Relations;
using Models.Db.RestaurantStop;
using Models.Db.Schedule;

namespace Models.Db.DbRestaurant
{
    public class Restaurant : IdEntity
    {
        [ForeignKey(nameof(City))]
        public long CityId { get; set; }

        public virtual City City { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(128)]
        public string Address { get; set; }

        [MaxLength(128)]
        public string SberbankUsername { get; set; }
        
        [MaxLength(128)]
        public string SberbankPassword { get; set; }

        public PaymentTypeFlags SupportedPaymentTypes { get; set; }
        
        public virtual RestaurantLatLng Location { get; set; }

        public virtual ICollection<WorkerAccount> WorkerAccounts { get; set; }
        
        public virtual ICollection<WorkerAccountToRestaurant> WorkerAccountsRelation { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<PickupOpenCloseTime> PickupTimes { get; set; }
        public virtual ICollection<RestaurantPickupStop> PickupStops { get; set; }

        public virtual ICollection<DeliveryOpenCloseTime> DeliveryTimes { get; set; }
        public virtual ICollection<RestaurantDeliveryStop> DeliveryStops { get; set; }

        public virtual ICollection<DeliveryZoneLatLng> DeliveryZone { get; set; }
    }
}