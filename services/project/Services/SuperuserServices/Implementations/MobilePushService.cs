using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.MobilePushes;
using Models.DTOs.Misc;
using Models.DTOs.MobilePushes;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class MobilePushService : IMobilePushService
    {
        private readonly IMobilePushByCityRepository _mobilePushByCityRepository;
        private readonly IMobilePushByPriceGroupRepository _mobilePushByPriceGroupRepository;
        private readonly IFCMService _fcmService;
        private readonly IMapper _mapper;

        public MobilePushService(IMobilePushByCityRepository mobilePushByCityRepository, IMobilePushByPriceGroupRepository mobilePushByPriceGroupRepository, IFCMService fcmService, IMapper mapper)
        {
            _mobilePushByCityRepository = mobilePushByCityRepository;
            _mobilePushByPriceGroupRepository = mobilePushByPriceGroupRepository;
            _fcmService = fcmService;
            _mapper = mapper;
        }

        public async Task<MobilePushWithIdDto> GetByIdByCities(long id)
        {
            var mobilePush = await _mobilePushByCityRepository
                .GetByIdNonTracking(
                    id,
                    n => n.CitiesRelation
                );

            var mobilePushWithIdDto = _mapper.Map<MobilePushWithIdDto>(mobilePush);

            return mobilePushWithIdDto;
        }

        public async Task<MobilePushWithIdDto> GetByIdByPriceGroups(long id)
        {
            var mobilePush = await _mobilePushByPriceGroupRepository
                .GetByIdNonTracking(
                    id,
                    n => n.PriceGroupsRelation
                );

            var mobilePushWithIdDto = _mapper.Map<MobilePushWithIdDto>(mobilePush);

            return mobilePushWithIdDto;
        }

        public async Task<ICollection<MobilePushWithIdDto>> GetAllByCities()
        {
            var mobilePushes = await _mobilePushByCityRepository
                .GetManyReversedNonTracking(
                    null,
                    n => n.CitiesRelation
                );

            var mobilePushWithIdDtos = _mapper.Map<ICollection<MobilePushWithIdDto>>(mobilePushes);

            return mobilePushWithIdDtos;
        }

        public async Task<ICollection<MobilePushWithIdDto>> GetAllByPriceGroups()
        {
            var mobilePushes = await _mobilePushByPriceGroupRepository
                .GetManyReversedNonTracking(
                    null,
                    n => n.PriceGroupsRelation
                );

            var mobilePushWithIdDtos = _mapper.Map<ICollection<MobilePushWithIdDto>>(mobilePushes);

            return mobilePushWithIdDtos;
        }

        public async Task DeleteByCities(long id)
        {
            var mobilePushByCity = await _mobilePushByCityRepository.GetById(id);

            await _mobilePushByCityRepository.Remove(mobilePushByCity);
        }

        public async Task DeleteByPriceGroups(long id)
        {
            var mobilePushByPriceGroup = await _mobilePushByPriceGroupRepository.GetById(id);

            await _mobilePushByPriceGroupRepository.Remove(mobilePushByPriceGroup);
        }

        public async Task<CreatedDto> CreateByCities(CreateMobilePushDto createMobilePushDto, bool send = true)
        {
            var mobilePush = _mapper.Map<MobilePushByCity>(createMobilePushDto);

            mobilePush.CreatedAt = DateTime.Now;

            await _mobilePushByCityRepository.Add(mobilePush);

            if (send)
            {
                await _fcmService.SendByCities(createMobilePushDto);
            }

            return mobilePush.Id;
        }

        public async Task<CreatedDto> CreateByPriceGroups(CreateMobilePushDto createMobilePushDto, bool send = true)
        {
            var mobilePush = _mapper.Map<MobilePushByPriceGroup>(createMobilePushDto);

            mobilePush.CreatedAt = DateTime.Now;

            await _mobilePushByPriceGroupRepository.Add(mobilePush);

            if (send)
            {
                await _fcmService.SendByPriceGroups(createMobilePushDto);
            }

            return mobilePush.Id;
        }
    }
}