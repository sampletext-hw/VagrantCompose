using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.Favorite;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class FavoriteController : AkianaMobileController
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Принимает всё избранное пользователя от мобилки (с шифрацией)")]
        public async Task<ActionResult> Upload([ModelBinder(typeof(EncodedJsonBinder))] UploadFavoriteItemDto uploadFavoriteItemDto)
        {
            await _favoriteService.Upload(uploadFavoriteItemDto);

            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Добавляет 1 позицию в избранное (с шифрацией)")]
        public async Task<ActionResult> Add([ModelBinder(typeof(EncodedJsonBinder))] AddFavoriteItemDto addFavoriteItemDto)
        {
            await _favoriteService.AddItem(addFavoriteItemDto);

            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Удаляет 1 позицию из избранного (с шифрацией)")]
        public async Task<ActionResult> Remove([ModelBinder(typeof(EncodedJsonBinder))] RemoveFavoriteItemDto removeFavoriteItemDto)
        {
            await _favoriteService.RemoveItem(removeFavoriteItemDto);

            return Ok();
        }

        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Очищает избранное у пользователя (с шифрацией)")]
        public async Task<ActionResult> Clear([Id(typeof(ClientAccount))] long id)
        {
            await _favoriteService.ClearByClient(id);

            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает все элементы избранного для пользователя (с шифрацией)")]
        public async Task<ActionResult<ICollection<MobileFavoriteItemDto>>> GetByClient([Id(typeof(ClientAccount))] long id)
        {
            var favoriteItemDtos = await _favoriteService.GetByClient(id);

            return Ok(favoriteItemDtos);
        }
    }
}