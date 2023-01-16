using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Db.LatLngs
{
    public class DeliveryAddressLatLng : LatLngBase
    {
        [ForeignKey(nameof(DeliveryAddress))]
        public long DeliveryAddressId { get; set; }

        public virtual DeliveryAddress DeliveryAddress { get; set; }
    }
}