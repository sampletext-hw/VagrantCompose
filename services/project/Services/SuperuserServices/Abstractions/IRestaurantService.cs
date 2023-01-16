using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.General;
using Models.DTOs.Misc;
using Models.DTOs.Restaurants;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = RestaurantWithIdDto;
    using TCreateDto = CreateRestaurantDto;
    using TUpdateDto = UpdateRestaurantDto;

    public interface IRestaurantService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<TWithIdDto>> GetByCity(long id);
    }
}