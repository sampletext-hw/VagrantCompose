using System.Threading.Tasks;
using Models.DTOs.External;

namespace Services.ExternalServices.Abstractions;

public interface IPaymentCallbackService
{
    Task Process(SberbankCallbackDto callbackDto);
}