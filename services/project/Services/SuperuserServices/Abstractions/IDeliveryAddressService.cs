using System.Threading.Tasks;
using Models.DTOs;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    public interface IDeliveryAddressService
    {
        Task<DeliveryAddressDto> GetById(long id);
    }
}