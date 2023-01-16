using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Carts;

namespace Services.MobileServices.Abstractions
{
    public interface ICartService
    {
        Task Upload(UploadCartItemsDto uploadCartItemsDto);
        
        Task AddItem(AddCartItemDto addCartItemDto);

        Task RemoveItem(RemoveCartItemDto removeCartItemDto);

        Task ClearByClient(long id);

        Task<ICollection<MobileCartItemDto>> GetByClient(long id);
    }
}