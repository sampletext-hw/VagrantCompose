using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.DTOs.Misc;
using Models.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services;
using Services.Shared.Abstractions;
using WebAPI.Utils;

namespace WebAPI.Filters
{
    public class SerializeOutputFilter : IAsyncActionFilter
    {
        private readonly IRequestAccountIdService _requestAccountIdService;

        public SerializeOutputFilter(IRequestAccountIdService requestAccountIdService)
        {
            _requestAccountIdService = requestAccountIdService;
        }

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings().Configure();
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Exception is not null)
            {
                return;
            }

            if (_requestAccountIdService.IsSet && !_requestAccountIdService.HasFullAccess ||
                context.HttpContext.Request.Query.ContainsKey("disable-aes"))
            {
                // если запрос с ключом авторизации и со старым типом авторизации, не сериализуем ему ответ
                return;
            }

            // для запросов без ключа авторизации или для новой авторизации

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                string json = JsonConvert.SerializeObject(okObjectResult.Value, Formatting.None, JsonSerializerSettings);

                var bytes = Encoding.UTF8.GetBytes(json);

                using var md5 = MD5.Create();

                var hashBytes = md5.ComputeHash(bytes);

                var (enc, iv) = AesConverter.Serialize(bytes);

                okObjectResult.Value = new JsonEncodedData()
                {
                    Data = Convert.ToBase64String(enc),
                    Key = Convert.ToBase64String(iv),
                    Hash = Convert.ToBase64String(hashBytes)
                };
            }
            else if (executedContext.Result is OkResult okResult)
            {
                // Do nothing
            }
            else
            {
                throw new AkianaException("Not a OkObjectResult Returned");
            }
        }
    }
}