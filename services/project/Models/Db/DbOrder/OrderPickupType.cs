using System;

namespace Models.Db.DbOrder
{
    public enum OrderPickupType : uint
    {
        Pickup = 1,
        Delivery = 2
    }

    public static class OrderPickupTypeExtensions
    {
        public static string ToFriendlyString(this OrderPickupType orderPickupType)
        {
            return orderPickupType switch
            {
                OrderPickupType.Pickup => "Самовывоз",
                OrderPickupType.Delivery => "Доставка",
                _ => throw new ArgumentOutOfRangeException(nameof(orderPickupType), orderPickupType, null)
            };
        }
    }
}