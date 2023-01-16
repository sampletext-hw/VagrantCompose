using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Models.DTOs.Misc;
using Services.ExternalServices;

namespace WebAPI.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        private ILogger<ValidateModelFilter> _logger;

        public ValidateModelFilter(ILogger<ValidateModelFilter> logger)
        {
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                string message = string.Join(
                    "\n",
                    context
                        .ModelState
                        .Select(e => e.Value.Errors)
                        .Where(e => e.Count > 0)
                        .SelectMany(e => e)
                        .Select(e => e.ErrorMessage)
                );
                context.Result = new BadRequestObjectResult(
                    new ErrorDto(message)
                );
                _logger.LogError("Model Validation Failed {path} {message}", context.HttpContext.Request.Path.ToString(), message);

                await TelegramAPI.Send(
                    $"{context.HttpContext.Request.Path.ToString()}\n" +
                    $"Model Validation Failed:\n" +
                    $"{message}"
                );
                return;
            }

            await next();
        }
    }
}