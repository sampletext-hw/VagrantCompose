using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Models.Configs;
using Models.Db.DbOrder;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace WebAPI.HostedServices;

public class OrderPostProcessingHostedService : BackgroundService
{
    private readonly IOrderPostProcessingQueue _orderPostProcessingQueue;
    private readonly IOrderPostProcessService _orderPostProcessService;
    private readonly IServiceProvider _serviceProvider;
    private readonly EmailServiceConfig _emailServiceConfig;
    public OrderPostProcessingHostedService(IOrderPostProcessingQueue orderPostProcessingQueue, IOrderPostProcessService orderPostProcessService, IServiceProvider serviceProvider, IOptions<EmailServiceConfig> options)
    {
        _orderPostProcessingQueue = orderPostProcessingQueue;
        _orderPostProcessService = orderPostProcessService;
        _serviceProvider = serviceProvider;
         _emailServiceConfig = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var item = await _orderPostProcessingQueue.Dequeue(cancellationToken);

                // await TelegramAPI.Send($"Dequeued OrderQueueItem: {item.OrderId}");

                using var serviceScope = _serviceProvider.CreateScope();
                var orderRepository = serviceScope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = await orderRepository.GetFullInfo(item.OrderId);

                if (order.PostProcessingStatus != PostProcessingStatus.Processed)
                {
                    await TelegramAPI.Send($"PostProcessing OrderQueueItem: {item.OrderId}");
                    await _orderPostProcessService.PostProcessOrder(order);
                    order.PostProcessingStatus = PostProcessingStatus.Processed;
                    await orderRepository.SaveChanges();
                    
                    await Task.Delay(_emailServiceConfig.DelayBetweenEmailsMs, cancellationToken);
                    // await TelegramAPI.Send($"Finished OrderQueueItem: {item.OrderId}");
                }
                else
                {
                    await TelegramAPI.Send($"Skipped OrderQueueItem: {item.OrderId}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // don't die
        }
    }
}