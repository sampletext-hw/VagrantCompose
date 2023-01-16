using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Models.DTOs.Misc;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace WebAPI.Filters
{
    public class RolesFilterAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public RolesFilterAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
#if DEBUG
            Console.WriteLine("Skipping Role Check in DEBUG");
            await next.Invoke();
#else
            if (!context.HttpContext.TryGetAuthToken(out var authToken))
            {
                throw new("Roles Filter was called with no authToken.\nEnsure Endpoint has AuthTokenFilter!");
            }

            var tokenRoleStorageService = context.HttpContext.RequestServices.GetRequiredService<ITokenRoleCacheService>();

            if (await tokenRoleStorageService.HasAnyRole(authToken, _roles))
            {
                await next();
            }
            else
            {
                await TelegramAPI.Send($"Unauthorized call to {context.HttpContext.Request.Path}\n with token: {authToken}");
                context.Result = new UnauthorizedObjectResult(new ErrorDto("Sorry :(. You are not allowed to call this method"));
            }
#endif
        }
    }
}