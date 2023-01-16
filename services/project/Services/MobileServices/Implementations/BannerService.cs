using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.Banners;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
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

        public async Task<ICollection<BannerMobileDto>> GetByCity(long id)
        {
            var banners = await _bannerRepository.GetManyNonTracking(
                b => b.IsActive && 
                     b.CitiesRelation.Any(c => c.CityId == id)
            );

            var bannersDto = _mapper.Map<ICollection<BannerMobileDto>>(banners);

            return bannersDto;
        }
    }
}