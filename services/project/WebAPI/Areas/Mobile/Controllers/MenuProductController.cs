using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Menu;
using Models.DTOs.MenuProducts;
using Newtonsoft.Json;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class MenuProductController : AkianaMobileController
    {
        private readonly IMenuProductService _menuProductService;

        public MenuProductController(IMenuProductService menuProductService)
        {
            _menuProductService = menuProductService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию о продуктах одной позиции меню (с шифрацией)")]
        public async Task<ActionResult<ICollection<MenuProductWithIdDto>>> GetByMenuItem([Id(typeof(MenuItem))] long id)
        {
            var menuProductWithIdDtos = await _menuProductService.GetByMenuItem(id);
            return Ok(menuProductWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию о продуктах набора позиций меню (с шифрацией)")]
        public async Task<ActionResult<ICollection<MenuProductMobileDto>>> GetAllByMenuItem(string ids)
        {
            var longIds = JsonConvert.DeserializeObject<long[]>(ids);
            var menuProductWithIdDtos = await _menuProductService.GetByMenuItems(longIds);
            return Ok(menuProductWithIdDtos);
        }
    }
}