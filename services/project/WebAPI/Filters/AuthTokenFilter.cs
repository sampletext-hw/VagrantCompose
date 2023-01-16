using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Models.DTOs.Misc;
using Services.ExternalServices;
using Services.Shared.Abstractions;
using WebAPI.Utils;

namespace WebAPI.Filters
{
    public class AuthTokenFilter : IAsyncResourceFilter
    {
        private readonly ITokenSessionService _tokenSessionService;
        private readonly IRequestAccountIdSetterService _requestAccountIdSetterService;
        private ILogger<AuthTokenFilter> _logger;

        public AuthTokenFilter(ITokenSessionService tokenSessionService, IRequestAccountIdSetterService requestAccountIdSetterService, ILogger<AuthTokenFilter> logger)
        {
            _tokenSessionService = tokenSessionService;
            _requestAccountIdSetterService = requestAccountIdSetterService;
            _logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
#if DEBUG
            // Console.WriteLine("Skipping Auth-Token Check in DEBUG");
            await next.Invoke();
            return;
#else

            string authToken = "";

            try
            {
                // Console.WriteLine("Performing Auth-Token Check in RELEASE");
                if (!context.HttpContext.TryGetAuthToken(out authToken))
                {
                    _logger.LogError("Auth token missing {path}", context.HttpContext.Request.Path.ToString());
                    context.Result = new UnauthorizedObjectResult(new ErrorDto(VMessages.AuthTokenMissing));
                    await TelegramAPI.Send(
                        $"{context.HttpContext.Request.Path.ToString()}\n" +
                        $"Attempt to access auth-token protected method without auth-token!"
                    );
                    return;
                }
            }
            catch (CryptographicException e)
            {
                _logger.LogError(e, "Failed to deserialize token {path}", context.HttpContext.Request.Path.ToString());
                await TelegramAPI.Send(
                    $"{context.HttpContext.Request.Path.ToString()}\n" +
                    $"Attempt to access auth-token protected method with incorrectly serialized auth-token!\n" +
                    $"{authToken}"
                );
                return;
            }

            if (string.IsNullOrEmpty(authToken))
            {
                _logger.LogError("Auth Token Empty {path}", context.HttpContext.Request.Path.ToString());
                context.Result = new UnauthorizedObjectResult(new ErrorDto(VMessages.AuthTokenEmpty));
                await TelegramAPI.Send(
                    $"{context.HttpContext.Request.Path.ToString()}\n" +
                    $"Attempt to access auth-token protected method with empty auth-token!"
                );
                return;
            }

            var accountSession = await _tokenSessionService.GetByToken(authToken);
            if (accountSession == null)
            {
                _logger.LogError("Account Session was not found for token {path} {token}", context.HttpContext.Request.Path.ToString(), authToken);
                context.Result = new UnauthorizedObjectResult(new ErrorDto(VMessages.AuthTokenUnknown));
                await TelegramAPI.Send(
                    $"{context.HttpContext.Request.Path.ToString()}\n" +
                    $"Attempt to access auth-token protected method with unknown auth-token\n" +
                    $"\"{authToken}\"!"
                );
                return;
            }

            if (accountSession.EndDate < DateTime.Now)
            {
                _logger.LogError("Expired Token Request {path} {token}", context.HttpContext.Request.Path.ToString(), authToken);
                context.Result = new UnauthorizedObjectResult(new ErrorDto(VMessages.AuthTokenExpired));
                await TelegramAPI.Send(
                    $"{context.HttpContext.Request.Path.ToString()}\n" +
                    $"Attempt to access auth-token protected method with expired auth-token\n" +
                    $"\"{authToken}\"!"
                );
                return;
            }

            _requestAccountIdSetterService.Set(accountSession.WorkerAccountId, accountSession.HasFullAccess, accountSession.IsTechnical);


            await next.Invoke();
#endif
        }
    }
}