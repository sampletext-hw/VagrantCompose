using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models.Configs;
using Models.DTOs.External;
using Newtonsoft.Json;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace Services.CommonServices.Implementations
{
    public class PaymentService : IPaymentService
    {
        // test
        // private const string SberbankUrl = "3dsec.sberbank.ru";
        // private const string UserName = "T3257069063-api";
        // private const string Password = "T3257069063";

        // prod
        // private const string SberbankUrl = "securepayments.sberbank.ru";
        // private const string UserName = "bryansk-dodopizza-api";
        // private const string Password = "cMHg19Eo7dm";

        private PaymentServiceConfig _config;

        public PaymentService(IOptions<PaymentServiceConfig> config)
        {
            _config = config.Value;
        }

        public async Task<SberbankRegisterResultDto> CreatePayment(string username, string password, long orderId, float amount, string description, long clientId)
        {
            using var client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            string url = $"{_config.SberbankUrl}/payment/rest/register.do";

            var parameters = new Dictionary<string, string>()
            {
                {"userName", username},
                {"password", password},
                {"orderNumber", orderId.ToString()},
                {"amount", (amount * 100).ToString(CultureInfo.InvariantCulture)},
                {"currency", "643"}, // rub
                {"returnUrl", _config.SuccessUrl},
                {"failUrl", _config.FailUrl},
                {"description", description},
                {"language", "ru"},
                {"pageView", "MOBILE"},
                {"clientId", $"app_{clientId}"},
                {"sessionTimeoutSecs", "1200"}
                // {"dynamicCallbackUrl", "https://akiantr.ru"}
            };

            var response = await client.PostAsync(url, new FormUrlEncodedContent(parameters));

            var respMessage = await response.Content.ReadAsStringAsync();

            await TelegramAPI.Send(
                "Sberbank register.do\n" +
                respMessage
            );

            var sberbankRegisterResultDto = JsonConvert.DeserializeObject<SberbankRegisterResultDto>(respMessage, new JsonSerializerSettings().Configure());

            return sberbankRegisterResultDto;
        }

        public async Task<SberbankGetOrderStatusResultDto> GetStatus(string username, string password, string externalId)
        {
            using var client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            string url = $"{_config.SberbankUrl}/payment/rest/getOrderStatusExtended.do";

            var parameters = new Dictionary<string, string>()
            {
                {"userName", username},
                {"password", password},
                {"orderId", externalId},
                {"language", "ru"},
                // {"dynamicCallbackUrl", "https://akiantr.ru"}
            };

            var response = await client.PostAsync(url, new FormUrlEncodedContent(parameters));

            var respMessage = await response.Content.ReadAsStringAsync();

            var sberbankGetOrderStatusResultDto = JsonConvert.DeserializeObject<SberbankGetOrderStatusResultDto>(respMessage, new JsonSerializerSettings().Configure());

            return sberbankGetOrderStatusResultDto;
        }
    }
}