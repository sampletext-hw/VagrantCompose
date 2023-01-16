using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.DbRestaurant;
using Models.DTOs.Cities;
using Models.DTOs.LatLngDtos;
using Models.DTOs.Misc;
using Models.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IPriceGroupRepository _priceGroupRepository;

        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IPriceGroupRepository priceGroupRepository, IMapper mapper, IRestaurantRepository restaurantRepository)
        {
            _cityRepository = cityRepository;
            _priceGroupRepository = priceGroupRepository;
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<CityWithIdDto> GetById(long id)
        {
            var city = await _cityRepository.GetByIdNonTracking(id,
                c => c.PriceGroup,
                c => c.LatLng
            );

            var cityGetByIdResultDto = _mapper.Map<CityWithIdDto>(city);
            return cityGetByIdResultDto;
        }

        public async Task<ICollection<CityWithIdDto>> GetAll()
        {
            var cities = await _cityRepository.GetManyNonTracking(null,
                c => c.PriceGroup,
                c => c.LatLng
            );

            var cityWithIdDtos = _mapper.Map<ICollection<CityWithIdDto>>(cities);

            return cityWithIdDtos;
        }

        public async Task<ICollection<CityWithIdDto>> GetByPriceGroup(long priceGroupId)
        {
            var cities = await _cityRepository.GetManyNonTracking(c => c.PriceGroupId == priceGroupId);

            var cityWithIdDtos = _mapper.Map<ICollection<CityWithIdDto>>(cities);

            return cityWithIdDtos;
        }

        public async Task Update(UpdateCityDto updateCityDto)
        {
            var city = await _cityRepository.GetById(updateCityDto.Id,
                c => c.LatLng
            );

            _mapper.Map(updateCityDto, city);

            await _cityRepository.Update(city);
        }

        public async Task<CreatedDto> Create(CreateCityDto createCityDto)
        {
            var city = _mapper.Map<City>(createCityDto);

            await _cityRepository.Add(city);

            return city.Id;
        }

        public async Task Remove(long id)
        {
            var city = await _cityRepository.GetById(id);

            var count = await _restaurantRepository.Count(r => r.CityId == city.Id);

            if (count > 0)
            {
                throw new AkianaException($"Невозможно удалить этот город! К нему ещё привязаны рестораны!");
            }

            await _cityRepository.Remove(city);
        }
    }
}