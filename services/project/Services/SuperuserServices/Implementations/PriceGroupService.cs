using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;
using Models.DTOs.PriceGroups;
using Models.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class PriceGroupService : IPriceGroupService
    {
        private readonly IPriceGroupRepository _priceGroupRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        private readonly IMapper _mapper;

        public PriceGroupService(IPriceGroupRepository priceGroupRepository, IMapper mapper, ICityRepository cityRepository, IMenuItemRepository menuItemRepository)
        {
            _priceGroupRepository = priceGroupRepository;
            _mapper = mapper;
            _cityRepository = cityRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<PriceGroupWithIdDto> GetById(long id)
        {
            var priceGroup = await _priceGroupRepository.GetByIdNonTracking(id);

            var priceGroupGetByIdResultDto = _mapper.Map<PriceGroupWithIdDto>(priceGroup);

            return priceGroupGetByIdResultDto;
        }

        public async Task<ICollection<PriceGroupWithIdDto>> GetAll()
        {
            var priceGroups = await _priceGroupRepository.GetManyNonTracking();

            var priceGroupWithIdDtos = _mapper.Map<ICollection<PriceGroupWithIdDto>>(priceGroups);

            return priceGroupWithIdDtos;
        }

        public async Task Update(UpdatePriceGroupDto updatePriceGroupDto)
        {
            var priceGroup = await _priceGroupRepository.GetById(updatePriceGroupDto.Id);

            _mapper.Map(updatePriceGroupDto, priceGroup);
            
            await _priceGroupRepository.Update(priceGroup);
        }

        public async Task<CreatedDto> Create(CreatePriceGroupDto createPriceGroupDto)
        {
            var priceGroup = _mapper.Map<PriceGroup>(createPriceGroupDto);

            await _priceGroupRepository.Add(priceGroup);

            return priceGroup.Id;
        }

        public async Task Remove(long id)
        {
            var priceGroup = await _priceGroupRepository.GetById(id);

            var cCount = await _cityRepository.Count(c => c.PriceGroupId == priceGroup.Id);

            if (cCount > 0)
            {
                throw new AkianaException($"Невозможно удалить эту ценовую группу! К ней ещё привязаны города!");
            }
            
            await _priceGroupRepository.Remove(priceGroup);
        }
    }
}