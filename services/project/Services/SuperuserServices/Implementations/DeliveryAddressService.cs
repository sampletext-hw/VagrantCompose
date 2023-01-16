using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs;
using Models.DTOs.DeliveryAddresses;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class DeliveryAddressService : IDeliveryAddressService
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;

        private readonly IMapper _mapper;

        public DeliveryAddressService(IDeliveryAddressRepository deliveryAddressRepository, IMapper mapper)
        {
            _deliveryAddressRepository = deliveryAddressRepository;
            _mapper = mapper;
        }

        public async Task<DeliveryAddressDto> GetById(long id)
        {
            var deliveryAddress = await _deliveryAddressRepository.GetByIdNonTracking(id);

            var deliveryAddressDto = _mapper.Map<DeliveryAddressDto>(deliveryAddress);

            return deliveryAddressDto;
        }
    }
}