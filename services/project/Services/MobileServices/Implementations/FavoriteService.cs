using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Models.Db.DbCart;
using Models.DTOs.Favorite;
using Models.Misc;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteItemRepository _favoriteItemRepository;

        private readonly IMapper _mapper;

        private ILogger<FavoriteService> _logger;

        public FavoriteService(IFavoriteItemRepository favoriteItemRepository, IMapper mapper, ILogger<FavoriteService> logger)
        {
            _favoriteItemRepository = favoriteItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Upload(UploadFavoriteItemDto uploadFavoriteItemsDto)
        {
            foreach (var addFavoriteItemDto in uploadFavoriteItemsDto.FavoriteItems)
            {
                await AddItem(addFavoriteItemDto);
            }
        }

        public async Task AddItem(AddFavoriteItemDto addFavoriteItemDto)
        {
            var favoriteItem = await _favoriteItemRepository.GetOne(i => i.ClientAccountId == addFavoriteItemDto.ClientAccountId && i.MenuItemId == addFavoriteItemDto.MenuItemId);

            if (favoriteItem != null)
            {
                _logger.LogInformation("Favorite Item is already favored {client_id} {menu_item}", addFavoriteItemDto.ClientAccountId, addFavoriteItemDto.MenuItemId);
                throw new AkianaException("Данная позиция меню уже в избранном у пользователя");
            }

            // Favorite item doen't exist, create one
            favoriteItem = _mapper.Map<FavoriteItem>(addFavoriteItemDto);
            await _favoriteItemRepository.Add(favoriteItem);
        }

        public async Task RemoveItem(RemoveFavoriteItemDto removeFavoriteItemDto)
        {
            var favoriteItem = await _favoriteItemRepository.GetOne(
                i =>
                    i.ClientAccountId == removeFavoriteItemDto.ClientAccountId &&
                    i.MenuItemId == removeFavoriteItemDto.MenuItemId
            );

            // Favorite item doesn't exist, this is an exception
            favoriteItem.EnsureNotNullHandled("Данная позиция меню не в избранном у пользователя");

            await _favoriteItemRepository.Remove(favoriteItem);
        }

        public async Task ClearByClient(long id)
        {
            var favoriteItems = await _favoriteItemRepository.GetMany(i => i.ClientAccountId == id);

            await _favoriteItemRepository.RemoveMany(favoriteItems);
        }

        public async Task<ICollection<MobileFavoriteItemDto>> GetByClient(long id)
        {
            var favoriteItems = await _favoriteItemRepository.GetManyNonTracking(i => i.ClientAccountId == id, i => i.MenuItem);

            var favoriteItemDtos = _mapper.Map<ICollection<MobileFavoriteItemDto>>(favoriteItems);

            return favoriteItemDtos;
        }
    }
}