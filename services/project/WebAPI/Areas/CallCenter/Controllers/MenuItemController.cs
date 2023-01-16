using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;
using Services.CallCenterServices.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Filters;

namespace WebAPI.Areas.CallCenter.Controllers
{
    public class MenuItemController : AkianaCallCenterController
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.CallCenter)]
        public async Task<ActionResult<ICollection<MenuItemWithIdDto>>> GetMany([FromBody] IdsDto ids)
        {
            var menuItemWithIdDtos = await _menuItemService.GetMany(ids);
            return Ok(menuItemWithIdDtos);
        }
    }
}