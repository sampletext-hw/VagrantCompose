using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = MenuProductWithIdDto;
    using TCreateDto = CreateMenuProductDto;
    using TUpdateDto = UpdateMenuProductDto;

    public interface IMenuProductService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<MenuProductWithIdDto>> GetByMenuItem(long id);

        Task<ICollection<MenuProductWithIdDto>> GetByCategory(long id);

        Task<ICollection<MenuProductWithIdDto>> GetMany(IdsDto idsDto);
    }
}