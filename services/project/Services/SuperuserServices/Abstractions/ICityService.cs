using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Cities;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = CityWithIdDto;
    using TCreateDto = CreateCityDto;
    using TUpdateDto = UpdateCityDto;

    public interface ICityService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<TWithIdDto>> GetByPriceGroup(long priceGroupId);
    }
}