using System.Collections.Generic;

namespace Infrastructure.Verbatims
{
    // ReSharper disable once InconsistentNaming
    public class VRoles
    {
        public const string Superuser = nameof(Superuser);
        public const string Cook = nameof(Cook);
        public const string CookHot = nameof(CookHot);
        public const string Packer = nameof(Packer);
        public const string DeliveryCashier = nameof(DeliveryCashier);
        public const string Manager = nameof(Manager);
        public const string Courier = nameof(Courier);
        public const string IPhoneApplication = nameof(IPhoneApplication);
        public const string AndroidApplication = nameof(AndroidApplication);
        public const string CallCenter = nameof(CallCenter);
        public const string Franchisee = nameof(Franchisee);

        public static Dictionary<string, string> EnToRu = new()
        {
            {Superuser, "Супер-пользователь"},
            {Cook, "Повар"},
            {CookHot, "Повар горячего цеха"},
            {Packer, "Упаковщик"},
            {DeliveryCashier, "Кассир доставки"},
            {Manager, "Менеджер"},
            {Courier, "Курьер"},
            {IPhoneApplication, "Приложение IPhone"},
            {AndroidApplication, "Приложение Android"},
            {CallCenter, "Колл-центр"},
            {Franchisee, "Франчайзи"},
        };

        public static Dictionary<string, string> RuToEn = new()
        {
            {"Супер-пользователь", Superuser},
            {"Повар", Cook},
            {"Повар горячего цеха", CookHot},
            {"Упаковщик", Packer},
            {"Кассир доставки", DeliveryCashier},
            {"Менеджер", Manager},
            {"Курьер", Courier},
            {"Приложение IPhone", IPhoneApplication},
            {"Приложение Android", AndroidApplication},
            {"Колл-центр", CallCenter},
            {"Франчайзи", Franchisee},
        };
    }
}