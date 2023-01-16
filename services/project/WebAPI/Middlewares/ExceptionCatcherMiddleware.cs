using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Models.DTOs.Misc;
using Models.Misc;
using Newtonsoft.Json;
using Services;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace WebAPI.Middlewares
{
    public class ExceptionCatcherMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IRequestCounterService _requestCounterService;

        private readonly ILogger<ExceptionCatcherMiddleware> _logger;

        public ExceptionCatcherMiddleware(RequestDelegate next, IRequestCounterService requestCounterService, ILogger<ExceptionCatcherMiddleware> logger)
        {
            _next = next;
            _requestCounterService = requestCounterService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (AkianaException ex)
            {
                await HandleAkianaExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleUnknownExceptionAsync(context, ex);
            }
        }

        private async Task HandleAkianaExceptionAsync(HttpContext context, AkianaException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            
            await context.Response.WriteAsJsonAsync(new ErrorDto(exception.Message));
        }

        private async Task HandleUnknownExceptionAsync(HttpContext context, Exception exception)
        {
            // var webClient = new WebClient();
            // var remoteIp = context.Connection.RemoteIpAddress!.ToString().Split(':').Last();
            // var ipLocation = await webClient.DownloadStringTaskAsync($"https://api.iplocation.net/?ip={remoteIp}");
            //
            // var deserializeObject = JsonConvert.DeserializeObject<dynamic>(ipLocation);
            // var isp = deserializeObject.isp;
            // var country_name = deserializeObject.country_name;

            if (exception.Message.StartsWith("The SPA default page middleware could not return the default page '/index.html'"))
            {
                _requestCounterService.NoticeIndexNotFound();

                var headers = string.Join("\n",
                    from header in context.Request.Headers
                    select "\t" + header.Key + " - " + string.Join(", ", header.Value.ToArray()));

                await TelegramAPI.Send(
                    $"index.html NotFound exception!\n" +
                    $"{context.Request.Path}\n" +
                    $"Headers:\n" +
                    $"{headers}\n" +
                    $"Method: {context.Request.Method}\n" //+
                    // $"IP: {remoteIp}\n" +
                    // $"ISP: {isp}\n" +
                    // $"Country: {country_name}"
                );
            }
            else
            {
                await TelegramAPI.Send(
                    $"Exception:\n" +
                    $"{context.Request.Path}\n" +
                    $"Method: {context.Request.Method}\n" +
                    // $"IP: {remoteIp}\n" +
                    // $"ISP: {isp}\n" +
                    // $"Country: {country_name}" +
                    exception.ToPrettyString()
                );
            }

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            _logger.LogCritical(exception, "Unprocessed Exception: {Message}", exception.Message);

            await context.Response.WriteAsJsonAsync(new ErrorDto("Извините, произошла непредвиденная ошибка. Разработчики уже уведомлены."));
        }
    }
}