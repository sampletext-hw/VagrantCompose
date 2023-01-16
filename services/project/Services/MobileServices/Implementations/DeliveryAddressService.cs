using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
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

        public async Task<CreatedDto> Add(AddDeliveryAddressDto addDeliveryAddressDto)
        {
            var deliveryAddress = _mapper.Map<DeliveryAddress>(addDeliveryAddressDto);

            await _deliveryAddressRepository.Add(deliveryAddress);

            return deliveryAddress.Id;
        }

        public async Task Update(UpdateDeliveryAddressDto updateDeliveryAddressDto)
        {
            var deliveryAddress = await _deliveryAddressRepository.GetById(
                updateDeliveryAddressDto.Id,
                a => a.LatLng
            );

            _mapper.Map(updateDeliveryAddressDto, deliveryAddress);

            await _deliveryAddressRepository.Update(deliveryAddress);
        }

        public async Task<ICollection<DeliveryAddressMobileDto>> GetByClient(long id)
        {
            var deliveryAddresses = await _deliveryAddressRepository.GetManyNonTracking(
                a => a.ClientAccountId == id,
                a => a.LatLng
            );

            var deliveryAddressMobileDtos = _mapper.Map<ICollection<DeliveryAddressMobileDto>>(deliveryAddresses);

            return deliveryAddressMobileDtos;
        }

        public async Task Delete(long id)
        {
            var deliveryAddress = await _deliveryAddressRepository.GetById(id);
            await _deliveryAddressRepository.Remove(deliveryAddress);
        }
    }
}