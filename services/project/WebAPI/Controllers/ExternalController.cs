using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTOs.External;
using Services.ExternalServices.Abstractions;

namespace WebAPI.Controllers;

// TODO: Remove this Route Attribute, it is only required for Swagger
[Route("[controller]/[action]")]
[ResponseCache(NoStore = true, Duration = 0)]
public class ExternalController : Controller
{
    private readonly ILogger _logger;

    private readonly IPaymentCallbackService _paymentCallbackService;

    public ExternalController(ILogger<ExternalController> logger, IPaymentCallbackService paymentCallbackService)
    {
        _logger = logger;
        _paymentCallbackService = paymentCallbackService;
    }

    [HttpPost]
    public async Task<ActionResult> PaymentCallback([FromForm] SberbankCallbackDto callbackDto)
    {
        await _paymentCallbackService.Process(callbackDto);
        return Ok();
    }
}