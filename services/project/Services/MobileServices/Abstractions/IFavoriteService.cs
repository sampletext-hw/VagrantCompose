using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Favorite;

namespace Services.MobileServices.Abstractions
{
    public interface IFavoriteService
    {
        Task Upload(UploadFavoriteItemDto uploadFavoriteItemsDto);
        
        Task AddItem(AddFavoriteItemDto addFavoriteItemDto);

        Task RemoveItem(RemoveFavoriteItemDto removeFavoriteItemDto);

        Task ClearByClient(long id);

        Task<ICollection<MobileFavoriteItemDto>> GetByClient(long id);
    }
}