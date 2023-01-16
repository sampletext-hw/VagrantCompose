using System.Threading.Tasks;

namespace Services.CommonServices.Abstractions
{
    public interface ISMSService
    {
        Task Send(string text, string recipient);
        Task<(bool result, int code)> SendCall(string recipient);
    }
}