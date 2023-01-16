using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;

namespace Services.CallCenterServices.Abstractions
{
    public interface IMenuProductService
    {
        Task<ICollection<MenuProductWithIdDto>> GetMany(IdsDto idsDto);
    }
}