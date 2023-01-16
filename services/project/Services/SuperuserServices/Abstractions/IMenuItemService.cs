using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Db.Menu;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = MenuItemWithIdDto;
    using TCreateDto = CreateMenuItemDto;
    using TUpdateDto = UpdateMenuItemDto;
    
    public interface IMenuItemService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<MenuItemWithIdDto>> GetByPriceGroup(long id);
        
        Task<ICollection<MenuItemWithIdDto>> GetByMenuProduct(long id);
        
        Task<ICollection<MenuItemWithIdDto>> GetByCategory(long id);

        Task<ICollection<MenuItemWithIdDto>> GetMany(IdsDto idsDto);
    }
}