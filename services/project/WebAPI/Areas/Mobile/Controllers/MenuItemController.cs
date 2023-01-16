using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Common;
using Models.Db.DbRestaurant;
using Models.DTOs.MenuItems;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class MenuItemController : AkianaMobileController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает все позиции меню по ценовой группе (с шифрацией)")]
        public async Task<ActionResult<ICollection<MenuItemMobileDto>>> GetByPriceGroup([Id(typeof(PriceGroup))] long id)
        {
            var menuItemWithIdDtos = await _menuItemService.GetByPriceGroup(id);
            return Ok(menuItemWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает все позиции меню по категории (с шифрацией)")]
        public async Task<ActionResult<ICollection<MenuItemMobileDto>>> GetByCategory([Id(typeof(Category))] long id)
        {
            var menuItemWithIdDtos = await _menuItemService.GetByCategory(id);
            return Ok(menuItemWithIdDtos);
        }
    }
}