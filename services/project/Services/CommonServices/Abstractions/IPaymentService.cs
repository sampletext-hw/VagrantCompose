using System.Threading.Tasks;
using Models.DTOs.External;

namespace Services.CommonServices.Abstractions
{
    public interface IPaymentService
    {
        Task<SberbankRegisterResultDto> CreatePayment(string username, string password, long orderId, float amount, string description, long clientId);
        Task<SberbankGetOrderStatusResultDto> GetStatus(string login, string password, string externalId);
    }
}