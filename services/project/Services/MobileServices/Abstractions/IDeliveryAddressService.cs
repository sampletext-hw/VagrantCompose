using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Services.MobileServices.Abstractions
{
    public interface IDeliveryAddressService
    {
        Task<CreatedDto> Add(AddDeliveryAddressDto addDeliveryAddressDto);

        Task Update(UpdateDeliveryAddressDto updateDeliveryAddressDto);

        Task<ICollection<DeliveryAddressMobileDto>> GetByClient(long id);

        Task Delete(long id);
    }
}