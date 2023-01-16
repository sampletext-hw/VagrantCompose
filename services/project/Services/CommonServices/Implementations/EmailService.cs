using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Models.Configs;
using Models.Db.Common;
using Models.Db.DbOrder;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace Services.CommonServices.Implementations
{
    public static class DateTimeExtensions
    {
        public static string ToFriendlyString(this DateTime dateTime)
        {
            string month = new[] {"Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря"}[(dateTime.Month - 1 + 12) % 12];
            return $"{dateTime.Day} {month} до {dateTime.TimeOfDay.ToString("hh\\:mm")!}";
        }
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        private readonly EmailServiceConfig _emailServiceConfig;

        public EmailService(IOptions<EmailServiceConfig> options, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _emailServiceConfig = options.Value;
        }

        private static (string tableTds, float totalCost) ProcessOrderItems(Order order)
        {
            var builder = new StringBuilder();
            HashSet<long> visitedMenuItemIds = new();
            float totalCost = 0;
            foreach (var orderItem in order.OrderItems)
            {
                if (!visitedMenuItemIds.Add(orderItem.MenuItemId))
                {
                    continue;
                }

                float amount = order.OrderItems.Count(i => i.MenuItemId == orderItem.MenuItemId);
                float price = orderItem.PurchasePrice;
                float? weight = orderItem.MenuItem.Measures.FirstOrDefault(m => m.MeasureType == MeasureType.Gram)?.Amount;

                builder.Append(@$"
                    <tr>
                        <td align=""left"" width=""20%"">{orderItem.MenuItem.Title}</td>
                        <td align=""center"">{(weight is null ? "не указано" : weight.ToString())} г.</td>{ /* This can be ignored */""}
                        <td align=""center"">{price}</td>{ /* This can be ignored */""}
                        <td align=""center"">{1}</td>{ /* This can be ignored */""}
                        <td align=""center"">{amount}</td>
                        <td align=""center"">{price * amount}</td>{ /* This can be ignored */""}
                    </tr>"
                );

                totalCost += price * amount;
            }

            return (builder.ToString(), totalCost);
        }

        public async Task SendOrderTechEmail(Order order)
        {
            try
            {
                await Send(order);
            }
            catch (Exception ex)
            {
                await TelegramAPI.Send(ex.ToPrettyString() ?? "Unrecoverable exception occured while sending email");
            }
        }

        private static string WrapHtmlB(string value)
        {
            return $"<b>{value}</b>";
        }

        private static string WrapHtmlBOrEmpty(string value, string prefix, bool includeBr = false)
        {
            return !string.IsNullOrEmpty(value) ? $"{WrapHtmlB($"{prefix}" + value)}{(includeBr ? "<br>" : "")}" : "";
        }

        private static string GetDeliveryAddressString(Order order)
        {
            string deliveryAddressString = "";

            if (order.PickupType == OrderPickupType.Pickup)
            {
                deliveryAddressString = WrapHtmlB(order.Restaurant.Address);
            }
            else
            {
                if (string.IsNullOrEmpty(order.DeliveryAddress.Street))
                {
                    throw new($"Order delivery address street was null or empty, OrderId: {order.Id}");
                }

                deliveryAddressString =
                    @$"{WrapHtmlB($"{order.DeliveryAddress.Street}")}<br>
                       {WrapHtmlBOrEmpty(order.DeliveryAddress.Home, "Дом: ", true)}
                       {WrapHtmlBOrEmpty(order.DeliveryAddress.Entrance, "Подъезд: ", true)}
                       {WrapHtmlBOrEmpty(order.DeliveryAddress.Flat, "Квартира/офис: ", true)}
                       {WrapHtmlBOrEmpty(order.DeliveryAddress.Floor, "Этаж: ", true)}                                         
                       {WrapHtmlBOrEmpty(order.DeliveryAddress.Comment, "Комментарий: ")}";
            }

            return deliveryAddressString;
        }

        private static string GetDeliveryDateTrOrEmpty(Order order)
        {
            string deliveryDateString = order.AwaitedAtDateTime.ToFriendlyString();

            return @$"<tr>
                <td>Время доставки/самовывоза</td>
                <td>
                <b>{deliveryDateString}</b>
                </td>
                </tr>";
        }

        public async Task Send(Order order)
        {
            var deliveryDateTr = GetDeliveryDateTrOrEmpty(order);

            var deliveryAddressString = GetDeliveryAddressString(order);

            var (tableTds, totalCost) = ProcessOrderItems(order);

            var mailMessage = @$"
<html>
    <head></head>
    <body>
        <div class=""mail"">
            <meta charset=""UTF-8"">
            <meta name=""format-detection"" content=""email=no"">
            <meta name=""format-detection"" content=""telephone=no"">
            <meta name=""format-detection"" content=""address=no"">
            <style>
               a.detection {{ color: white; text-decoration: none; pointer-events: none; }}
               #logo-eshop, #logo-eshop_mr_css_attr {{
                    height:auto !important
                }}

                #logo-eshop_mr_css_attr table, #logo-eshop table {{
                    color:#fff
                }}
            </style>
            <title></title>
            <div id=""logo-eshop"" style=""width:100%;height:40;background-color: #202338; color: white; padding: 0px 10px 0px;"">
               <p id=""logo"" style=""font-size:24px; padding: 0px;"">ESHOP</p>
               <p style=""font-size:16px; padding: 0px;"">Заказ № <a class=""detection"">{order.Id}</a></p>
               <table id=""table-order-user-info"" width=""100%"" style=""border-collapse: collapse; border: 0px solid black;"">
                  <tbody>
                     <tr style=""border-bottom: 0.5px solid white"">
                        <td colspan=""2"" style=""padding: 25px 0px 5px; font-size:20px"">Контакты пользователя</td>
                     </tr>
                     <tr>
                        <td width=""30%"">Имя</td>
                        <td>{WrapHtmlB(!string.IsNullOrEmpty(order.ClientAccount.Username) ? order.ClientAccount.Username : "не указано")}</td>
                     </tr>
                     <tr>
                        <td>Телефон</td>
                        <td><b><a class=""detection"">{order.ClientAccount.Login}</a></b></td>
                     </tr>
                     <tr style=""border-bottom: 0.5px solid white"">
                        <td colspan=""2"" style=""padding: 25px 0px 5px; font-size:20px"">Данные заказа</td>
                     </tr>
                     <tr>
                        <td>Тип оплаты</td>
                        <td>{WrapHtmlB(order.PaymentType.ToFriendlyString())}</td>
                     </tr>
                     <tr>
                        <td>Тип доставки</td>
                        <td>{WrapHtmlB(order.PickupType.ToFriendlyString())}</td>
                     </tr>
                     <tr>
                        <td valign=""top"">Адрес доставки/самовывоза</td>
                        <td>
                           {deliveryAddressString}
                        </td>
                     </tr>
                     {deliveryDateTr}
                  </tbody>
               </table>
            </div>
            <br>
            <table id=""table-order"" width=""100%"" style=""border-collapse: collapse; border: 1px solid grey;"">
               <tbody>
                  <tr>
                     <th align=""left"">Название</th>
                     <th>Описание</th>
                     <th>Цена</th>
                     <th>Мин. ед.</th>
                     <th>Кол-во</th>
                     <th>Общая стоимость</th>
                  </tr>
                  {tableTds}
               </tbody>
            </table>
            <br>
            <table id=""table-order-info"" width=""100%"" style=""border-collapse: collapse; border: 0px solid grey;"">
               <tbody>
                  <tr>
                     <td width=""30%""><b>Кол-во товаров</b></td>
                     <td align=""left"">{order.OrderItems.Count}</td>
                  </tr>
                  <tr>
                     <td width=""30%""><b>Стоимость заказа</b></td>
                     <td align=""left"">{totalCost} ₽</td>
                  </tr>
               </tbody>
            </table>
            <br>
            <p><b>Комментарий: </b>
            {(!string.IsNullOrEmpty(order.Promocode) ? $"Промокод: {order.Promocode}, " : "")}
            {(!string.IsNullOrEmpty(order.Comment) ? $"Комментарий: {order.Comment}, " : "")}
            Город: {order.Restaurant.City.Title}, Суши-бар: {order.Restaurant.Address}
            </p>
            <br>
            <div style=""width:100%; background-color: #202338; color: white;"">
               <p align=""center"">Хорошего дня! С уважением, команда IT-Park</p>
            </div>
         </div>
    </body>
</html>";

            if (_emailServiceConfig.SaveFile)
            {
                if (!Directory.Exists("order-emails"))
                {
                    Directory.CreateDirectory("order-emails");
                }

                await using var fileStream = File.Create($"order-emails/generated{order.Id}.html");

                await fileStream.WriteAsync(Encoding.UTF8.GetBytes(mailMessage));
            }

            foreach (var email in _emailServiceConfig.Emails)
            {
                var (success, exception) = _emailSender.SendOne(email, mailMessage, $"Новый заказ {order.Id}", true);
                if (!success)
                {
                    await TelegramAPI.Send($"Email sending failed to {email}!\n" + exception.Message);
                }
            }
        }
    }
}