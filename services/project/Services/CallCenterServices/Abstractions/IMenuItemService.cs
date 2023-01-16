using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;

namespace Services.CallCenterServices.Abstractions
{
    public interface IMenuItemService
    {
        Task<ICollection<MenuItemWithIdDto>> GetMany(IdsDto idsDto);
    }
}