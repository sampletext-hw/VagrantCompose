using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.Cities;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CityMobileDto>> GetAll()
        {
            var cities = await _cityRepository.GetManyNonTracking(c => c.LatLng);

            var cityMobileDtos = _mapper.Map<ICollection<CityMobileDto>>(cities);

            return cityMobileDtos;
        }
    }
}