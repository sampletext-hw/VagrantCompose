using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Models.Db.DbOrder;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace Services.CommonServices.Implementations;

public class OrderPostProcessService : IOrderPostProcessService
{
    private readonly IEmailService _emailService;
    private readonly ISSEService _sseService;
    private readonly IMobileIdStorageService _mobileIdStorageService;

    private ILogger<OrderPostProcessService> _logger;

    public OrderPostProcessService(IEmailService emailService, ISSEService sseService, IMobileIdStorageService mobileIdStorageService, ILogger<OrderPostProcessService> logger)
    {
        _emailService = emailService;
        _sseService = sseService;
        _mobileIdStorageService = mobileIdStorageService;
        _logger = logger;
    }

    public async Task PostProcessOrder(Order order)
    {
        await _emailService.SendOrderTechEmail(order);

        _sseService.EmitOrderCreated(order.Id);

        bool isAndroid = _mobileIdStorageService.IsAndroidId(order.CreatorWorkerAccountId);
        bool isIPhone = _mobileIdStorageService.IsIosId(order.CreatorWorkerAccountId);
        
        
        _logger.LogInformation("Order Created {order_id} {is_android} {is_ios}", order.Id, isAndroid, isIPhone);

        await TelegramAPI.Send($"Order created: {order.Id}\nSource: {(isAndroid ? "Android" : isIPhone ? "IPhone" : "Unknown")}");
    }
}