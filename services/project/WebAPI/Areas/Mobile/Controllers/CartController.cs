using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.Carts;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class CartController : AkianaMobileController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Принимает всё содержимое корзины от мобилки (с шифрацией)")]
        public async Task<ActionResult> Upload([ModelBinder(typeof(EncodedJsonBinder))] UploadCartItemsDto uploadCartItemsDto)
        {
            await _cartService.Upload(uploadCartItemsDto);

            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Добавляет n элементов одной позиции в корзину (с шифрацией)")]
        public async Task<ActionResult> Add([ModelBinder(typeof(EncodedJsonBinder))] AddCartItemDto addCartItemDto)
        {
            await _cartService.AddItem(addCartItemDto);

            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Удаляет n элементов одной позиции из корзины (с шифрацией)")]
        public async Task<ActionResult> Remove([ModelBinder(typeof(EncodedJsonBinder))] RemoveCartItemDto removeCartItemDto)
        {
            await _cartService.RemoveItem(removeCartItemDto);

            return Ok();
        }

        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [SwaggerOperation("Очищает корзину пользователя (с шифрацией)")]
        public async Task<ActionResult> Clear([Id(typeof(ClientAccount))] long id)
        {
            await _cartService.ClearByClient(id);

            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает содержимое корзины пользователя (с шифрацией)")]
        public async Task<ActionResult<ICollection<MobileCartItemDto>>> GetByClient([Id(typeof(ClientAccount))] long id)
        {
            var cartItemDtos = await _cartService.GetByClient(id);

            return Ok(cartItemDtos);
        }
    }
}