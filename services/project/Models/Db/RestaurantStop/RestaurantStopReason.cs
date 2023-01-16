using System;

namespace Models.Db.RestaurantStop
{
    public enum RestaurantStopReason : uint
    {
        CouriersShortage = 1,
        KitchenWorkersShortage = 2,
        TechBreakage = 3,
        ElectricityWaterGasShortage = 4,
        InternetShortage = 5,
        Reconstruction = 6
    }

    public static class RestaurantStopReasonExtensions
    {
        public static string ToFriendlyString(this RestaurantStopReason reason)
        {
            return reason switch
            {
                RestaurantStopReason.CouriersShortage => "Нехватка курьеров",
                RestaurantStopReason.KitchenWorkersShortage => "Нехватка работников на кухне",
                RestaurantStopReason.TechBreakage => "Поломка оборудования",
                RestaurantStopReason.ElectricityWaterGasShortage => "Отсутствие электричества/воды/газа",
                RestaurantStopReason.InternetShortage => "Отсутствие доступа в интернет",
                RestaurantStopReason.Reconstruction => "Строительные работы",
                _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
            };
        }
    }
}