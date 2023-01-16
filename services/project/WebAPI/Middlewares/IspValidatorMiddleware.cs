using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Services.ExternalServices;
using Services.Shared.Abstractions;

namespace WebAPI.Middlewares
{
    public class IspValidatorMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IIPCache _ipCache;

        public IspValidatorMiddleware(RequestDelegate next, IIPCache ipCache)
        {
            _next = next;
            _ipCache = ipCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var (isBlocked, isp) = await _ipCache.GetIsValidISP(ip);
            if (isBlocked)
            {
                await TelegramAPI.Send($"{isp} IP Blocked: {ip}");
                context.Abort();
            }
            else
            {
                await _next(context);
            }
        }
    }
}