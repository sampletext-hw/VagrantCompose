using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.Favorite
{
    public class UploadFavoriteItemDto : IDto
    {
        public ICollection<AddFavoriteItemDto> FavoriteItems { get; set; }
    }
}