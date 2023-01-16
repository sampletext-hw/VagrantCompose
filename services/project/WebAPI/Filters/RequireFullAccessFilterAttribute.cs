using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Models.DTOs.Misc;
using Services.ExternalServices;
using Services.Shared.Abstractions;

namespace WebAPI.Filters
{
    public class RequireFullAccessFilter : IAsyncActionFilter
    {
        private readonly IRequestAccountIdService _requestAccountIdService;
        private ILogger<RequireFullAccessFilter> _logger;

        public RequireFullAccessFilter(IRequestAccountIdService requestAccountIdService, ILogger<RequireFullAccessFilter> logger)
        {
            _requestAccountIdService = requestAccountIdService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!_requestAccountIdService.HasFullAccess)
            {
                _logger.LogError("RequireFullAccess Violation {path}", context.HttpContext.Request.Path.ToString());
                context.Result = new UnauthorizedObjectResult(new ErrorDto(VMessages.RequiresFullAccessErrorMessage));
                await TelegramAPI.Send($"{context.HttpContext.Request.Path.ToString()}\nFull Access Violation");
                return;
            }
            await next();
        }
    }
}