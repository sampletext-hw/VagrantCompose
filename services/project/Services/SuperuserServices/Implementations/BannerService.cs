using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db;
using Models.DTOs.Banners;
using Models.DTOs.General;
using Models.DTOs.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        private readonly IMapper _mapper;

        public BannerService(IBannerRepository bannerRepository, IMapper mapper)
        {
            _bannerRepository = bannerRepository;
            _mapper = mapper;
        }

        public async Task<BannerWithIdDto> GetById(long id)
        {
            var banner = await _bannerRepository.GetByIdNonTracking(id, b => b.CitiesRelation);

            var bannerWithIdDto = _mapper.Map<BannerWithIdDto>(banner);

            return bannerWithIdDto;
        }

        public async Task<ICollection<BannerWithIdDto>> GetAll()
        {
            var banners = await _bannerRepository.GetManyReversedNonTracking(null, b => b.CitiesRelation);

            var bannersDto = _mapper.Map<ICollection<BannerWithIdDto>>(banners);

            return bannersDto;
        }

        public async Task Update(UpdateBannerDto updateDto)
        {
            var banner = await _bannerRepository.GetById(
                updateDto.Id,
                b => b.CitiesRelation
            );

            banner.CitiesRelation.Clear();
            
            _mapper.Map(updateDto, banner);

            await _bannerRepository.Update(banner);
        }

        public async Task<CreatedDto> Create(CreateBannerDto createDto)
        {
            var banner = _mapper.Map<Banner>(createDto);

            await _bannerRepository.Add(banner);

            return banner.Id;
        }

        public async Task Remove(long id)
        {
            var banner = await _bannerRepository.GetById(id);

            await _bannerRepository.Remove(banner);
        }

        public async Task<ICollection<BannerWithIdDto>> GetByCity(long id)
        {
            var banners = await _bannerRepository.GetManyReversedNonTracking(
                b => b.CitiesRelation.Any(c => c.CityId == id),
                b => b.CitiesRelation
            );

            var bannersDto = _mapper.Map<ICollection<BannerWithIdDto>>(banners);

            return bannersDto;
        }

        public async Task<IsActiveDto> ToggleActive(long id)
        {
            var banner = await _bannerRepository.GetById(id);

            banner.IsActive = !banner.IsActive;

            await _bannerRepository.Update(banner);

            return banner.IsActive;
        }
    }
}