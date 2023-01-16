using System.Threading.Tasks;
using Models.DTOs.MobilePushes;

namespace Services.CommonServices.Abstractions
{
    public interface IFCMService
    {
        Task<string> SendByCities(CreateMobilePushDto createMobilePushDto);
        Task<string> SendByPriceGroups(CreateMobilePushDto createMobilePushDto);
    }
}