using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Db.DbOrder
{
    [Flags]
    public enum PaymentTypeFlags : uint
    {
        Undefined = 0,
        CardToCourier = 1,
        CashToCourier = 2,
        Online = 4,
    }
    
    public enum LegacyPaymentType : uint
    {
        Undefined = 0,
        CardToCourier = 1,
        CashToCourier = 2,
        Online = 3,
    }

    public static class PaymentTypeExtensions
    {
        public static string ToFriendlyString(this PaymentTypeFlags paymentType)
        {
            return paymentType switch
            {
                PaymentTypeFlags.CardToCourier => "Безналичный расчет при получении",
                PaymentTypeFlags.CashToCourier => "Наличный расчет",
                PaymentTypeFlags.Online => "Оплата картой на сайте",
                _ => throw new ArgumentOutOfRangeException(nameof(paymentType), paymentType, null)
            };
        }

        public static PaymentTypeFlags ToDb(this LegacyPaymentType legacyPaymentType)
        {
            return legacyPaymentType switch
            {
                LegacyPaymentType.Undefined => PaymentTypeFlags.Undefined,
                LegacyPaymentType.CardToCourier => PaymentTypeFlags.CardToCourier,
                LegacyPaymentType.CashToCourier => PaymentTypeFlags.CashToCourier,
                LegacyPaymentType.Online => PaymentTypeFlags.Online,
                _ => throw new ArgumentOutOfRangeException(nameof(legacyPaymentType), legacyPaymentType, null)
            };
        }

        public static ICollection<PaymentTypeFlags> ToBits(this PaymentTypeFlags paymentType)
        {
            return Enum.GetValues<PaymentTypeFlags>()
                .Where(v => (paymentType & v) != 0)
                .ToList();
        }

        public static PaymentTypeFlags FromBits(this ICollection<PaymentTypeFlags> paymentTypeBits)
        {
            return paymentTypeBits.Aggregate(PaymentTypeFlags.Undefined, (accum, curr) => accum | curr);
        }

        public static string ToReadableString(this ICollection<PaymentTypeFlags> paymentTypeBits)
        {
            return string.Join(", ", paymentTypeBits);
        }
    }
}