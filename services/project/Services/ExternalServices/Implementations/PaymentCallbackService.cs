using System;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Models.Db.Payments;
using Models.DTOs.External;
using Services.CommonServices.Abstractions;
using Services.ExternalServices.Abstractions;

namespace Services.ExternalServices.Implementations;

public class PaymentCallbackService : IPaymentCallbackService
{
    private readonly IOnlinePaymentRepository _onlinePaymentRepository;
    private readonly IOrderPostProcessingQueue _orderPostProcessingQueue;
    private readonly ICartItemRepository _cartItemRepository;

    public PaymentCallbackService(IOnlinePaymentRepository onlinePaymentRepository, IOrderPostProcessingQueue orderPostProcessingQueue, ICartItemRepository cartItemRepository)
    {
        _onlinePaymentRepository = onlinePaymentRepository;
        _orderPostProcessingQueue = orderPostProcessingQueue;
        _cartItemRepository = cartItemRepository;
    }

    public async Task Process(SberbankCallbackDto callbackDto)
    {
        // FAILING: 
        // OrderNumber: 25
        // MdOrder: e00ac2af-bfd5-7c49-ab49-bda3284b6ddc
        // Operation: declinedByTimeout
        // Status: 1
        
        // DECLINED:
        // OrderNumber: 27
        // MdOrder: 682bc15e-d159-7fc9-991b-48d4284b6ddc
        // Operation: deposited
        // Status: 0
        
        // SUCCEEDED:
        // OrderNumber: 28
        // MdOrder: 13c0fa2d-48de-778d-97f0-13db284b6ddc
        // Operation: deposited
        // Status: 1

        var onlinePayment = await _onlinePaymentRepository.GetOne(p => p.ExternalId == callbackDto.MdOrder && p.OrderId == callbackDto.OrderNumber);

        if (onlinePayment is null)
        {
            await TelegramAPI.Send(
                $"**SBERBANK CALLBACK PROCESSING ERROR**\n" +
                $"\tOnlinePayment Was Not Found For:\n" +
                $"\tPaymentId: {callbackDto.MdOrder}\n" +
                $"\tOrderId: {callbackDto.OrderNumber}"
            );
            return;
        }

        if (onlinePayment.ConfirmationSource != ConfirmationSource.Unconfirmed && onlinePayment.PaymentStatus == PaymentStatus.Payed)
        {
            await TelegramAPI.Send(
                $"**SBERBANK CALLBACK PROCESSING ERROR**\n" +
                $"\tOnlinePayment Was Already Confirmed Payed:\n" +
                $"\tPaymentId: {callbackDto.MdOrder}\n" +
                $"\tOrderId: {callbackDto.OrderNumber}"
            );
            return;
        }
        
        onlinePayment.ConfirmationSource = ConfirmationSource.Callback;

        if (callbackDto.Operation == "declinedByTimeout")
        {
            onlinePayment.PaymentStatus = PaymentStatus.Declined;

            await TelegramAPI.Send(
                $"**SBERBANK CALLBACK declinedByTimeout**\n" +
                $"\tPaymentId: {callbackDto.MdOrder}\n" +
                $"\tOrderId: {callbackDto.OrderNumber}"
            );
        }
        else if (callbackDto.Operation == "deposited")
        {
            switch (callbackDto.Status)
            {
                case 0:
                    
                    // В этом случае клиента перенаправляет на повторную оплату автоматом, поэтому нужно это учесть при следующем Callback
                    // Не меняем статус платежа, т.к. он по факту не изменился - оплаты не было и клиент на этапе оплаты

                    await TelegramAPI.Send(
                        $"**SBERBANK CALLBACK Declined**\n" +
                        $"\tPaymentId: {callbackDto.MdOrder}\n" +
                        $"\tOrderId: {callbackDto.OrderNumber}"
                    );
                    break;
                case 1:
                    onlinePayment.PaymentStatus = PaymentStatus.Payed;
                    onlinePayment.ConfirmedAt = DateTime.Now;
                    await TelegramAPI.Send(
                        $"**SBERBANK CALLBACK Success**\n" +
                        $"\tPaymentId: {callbackDto.MdOrder}\n" +
                        $"\tOrderId: {callbackDto.OrderNumber}"
                    );
                    
                    var cartItemsFromOnlinePayment = await _cartItemRepository.GetCartItemsFromOnlinePaymentId(onlinePayment.Id);

                    // Clear user's cart if it's not online payment
                    await _cartItemRepository.RemoveMany(cartItemsFromOnlinePayment);
                    
                    await _orderPostProcessingQueue.Enqueue(onlinePayment.IssuerId, onlinePayment.OrderId);
                    break;
                default:
                    onlinePayment.PaymentStatus = PaymentStatus.Error;
                    await TelegramAPI.Send(
                        $"**SBERBANK CALLBACK Unknown Status**\n" +
                        $"\tStatus: {callbackDto.Status}n" +
                        $"\tPaymentId: {callbackDto.MdOrder}\n" +
                        $"\tOrderId: {callbackDto.OrderNumber}"
                    );
                    break;
            }
        }
        else
        {
            await TelegramAPI.Send(
                $"**SBERBANK CALLBACK Unknown Operation**\n" +
                $"\tOperation: {callbackDto.Operation}n" +
                $"\tPaymentId: {callbackDto.MdOrder}\n" +
                $"\tOrderId: {callbackDto.OrderNumber}"
            );
        }

        await _onlinePaymentRepository.Update(onlinePayment);
        
        
    }
}