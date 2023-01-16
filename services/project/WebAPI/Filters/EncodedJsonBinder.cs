using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Services.ExternalServices;
using Services.Shared.Abstractions;
using WebAPI.Utils;

namespace WebAPI.Filters
{
    public class EncodedJsonBinder : IModelBinder
    {
        private IRequestAccountIdService _requestAccountIdService;

        public EncodedJsonBinder(IRequestAccountIdService requestAccountIdService)
        {
            _requestAccountIdService = requestAccountIdService;
        }

        public async Task BindModelAsync(ModelBindingContext context)
        {
            context.HttpContext.Request.EnableBuffering();

            var readResult = await context.HttpContext.Request.BodyReader.ReadAsync();

            var requestJson = Encoding.UTF8.GetString(readResult.Buffer);

            if (_requestAccountIdService.IsSet && !_requestAccountIdService.HasFullAccess ||
                context.HttpContext.Request.Query.ContainsKey("disable-aes"))
            {
                var data = JsonConvert.DeserializeObject(requestJson, context.ModelType);
                context.Result = ModelBindingResult.Success(data);
                return;
            }

            try
            {
                var jsonEncodedData = JsonConvert.DeserializeObject<JsonEncodedData>(requestJson);

                var dataBytes = Convert.FromBase64String(jsonEncodedData.Data);
                var dataIv = Convert.FromBase64String(jsonEncodedData.Key);
                var dataHash = Convert.FromBase64String(jsonEncodedData.Hash);

                var deserializedBytes = AesConverter.Deserialize(dataBytes, dataIv);

                using var md5 = MD5.Create();
                var hash = md5.ComputeHash(deserializedBytes);

                if (!hash.SequenceEqual(dataHash))
                {
                    context.Result = ModelBindingResult.Failed();
                    context.ModelState.AddModelError("Deserialize", "Hash mismatch");
                    return;
                }

                var requestString = Encoding.UTF8.GetString(deserializedBytes);

                try
                {
                    var model = JsonConvert.DeserializeObject(requestString, context.ModelType);

                    context.Result = ModelBindingResult.Success(model);
                }
                catch
                {
                    context.Result = ModelBindingResult.Failed();
                    context.ModelState.AddModelError("Deserialize", $"Неподдерживаемый тип: {requestString}");
                }
            }
            catch (CryptographicException)
            {
                context.Result = ModelBindingResult.Failed();
                context.ModelState.AddModelError("Deserialize", "Не удалось десериализовать данные");
                await TelegramAPI.Send(
                    "DESERIALIZATION FAIL:\n" +
                    "```\n" +
                    $"{requestJson}\n" +
                    "```"
                );
            }
            catch (Exception)
            {
                context.Result = ModelBindingResult.Failed();
                context.ModelState.AddModelError("Deserialize", "Не удалось десериализовать данные");
                await TelegramAPI.Send(
                    "DESERIALIZATION FAIL:\n" +
                    "```\n" +
                    $"{requestJson}\n" +
                    "```"
                );
            }
        }
    }
}