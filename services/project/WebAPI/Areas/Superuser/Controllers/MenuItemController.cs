using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Common;
using Models.Db.DbRestaurant;
using Models.Db.Menu;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;
using Models.Misc;
using Services;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class MenuItemController : CrudController<MenuItem, MenuItemWithIdDto, CreateMenuItemDto, UpdateMenuItemDto>
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IImageService _imageService;
        private const float MaxMenuItemImageSizeInMegabytes = 0.5f;

        public MenuItemController(IMenuItemService menuItemService, Func<Type, object, bool> existenceChecker, IImageService imageService) : base(menuItemService, existenceChecker)
        {
            _menuItemService = menuItemService;
            _imageService = imageService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuItemWithIdDto>>> GetByPriceGroup([Id(typeof(PriceGroup))] long id)
        {
            var menuItemWithIdDtos = await _menuItemService.GetByPriceGroup(id);
            return Ok(menuItemWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuItemWithIdDto>>> GetByMenuProduct([Id(typeof(MenuProduct))] long id)
        {
            var menuItemWithIdDtos = await _menuItemService.GetByMenuProduct(id);
            return Ok(menuItemWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuItemWithIdDto>>> GetByCategory([Id(typeof(Category))] long id)
        {
            var menuItemWithIdDtos = await _menuItemService.GetByCategory(id);
            return Ok(menuItemWithIdDtos);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuItemWithIdDto>>> GetMany([FromBody] IdsDto ids)
        {
            var menuItemWithIdDtos = await _menuItemService.GetMany(ids);
            return Ok(menuItemWithIdDtos);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ImageDto>> UploadImage(IFormFile image)
        {
            image.EnsureNotNullHandled("image is missing");

            await using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            if (ms.Length == 0)
            {
                throw new AkianaException("image was empty");
            }

            if (ms.Length > MaxMenuItemImageSizeInMegabytes * 1024 * 1024)
            {
                throw new AkianaException($"Размер изображения превышает максимальный ({MaxMenuItemImageSizeInMegabytes} Мб)");
            }

            ms.Position = 0;
            var imageName = await _imageService.Create(image.FileName, "MenuItem", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
    }
}