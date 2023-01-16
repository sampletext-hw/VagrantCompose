using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;
using Services.CallCenterServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.CallCenter.Controllers
{
    public class MenuProductController : AkianaCallCenterController
    {
        private readonly IMenuProductService _menuProductService;

        public MenuProductController(IMenuProductService menuProductService)
        {
            _menuProductService = menuProductService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.CallCenter)]
        public async Task<ActionResult<ICollection<MenuProductWithIdDto>>> GetMany([FromBody] IdsDto ids)
        {
            var menuItemWithIdDtos = await _menuProductService.GetMany(ids);
            return Ok(menuItemWithIdDtos);
        }
    }
}