using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.MenuItems;

namespace Services.MobileServices.Abstractions
{
    public interface IMenuItemService
    {
        Task<ICollection<MenuItemMobileDto>> GetByPriceGroup(long id);
        
        Task<ICollection<MenuItemMobileDto>> GetByCategory(long id);
    }
}