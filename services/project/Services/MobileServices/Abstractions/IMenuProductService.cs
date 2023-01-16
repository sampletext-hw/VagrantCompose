using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.MenuProducts;

namespace Services.MobileServices.Abstractions
{
    public interface IMenuProductService
    {
        Task<ICollection<MenuProductMobileDto>> GetByMenuItem(long id);

        Task<ICollection<MenuProductMobileDto>> GetByMenuItems(ICollection<long> ids);
    }
}