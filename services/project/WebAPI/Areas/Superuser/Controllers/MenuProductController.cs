using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Common;
using Models.Db.Menu;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;
using Models.Misc;
using Services;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class MenuProductController : CrudController<MenuProduct, MenuProductWithIdDto, CreateMenuProductDto, UpdateMenuProductDto>
    {
        private readonly IMenuProductService _menuProductService;
        private readonly IImageService _imageService;
        private const float MaxMenuProductImageSizeInMegabytes = 0.5f;

        public MenuProductController(IMenuProductService menuProductService, Func<Type, object, bool> existenceChecker, IImageService imageService) : base(menuProductService, existenceChecker)
        {
            _menuProductService = menuProductService;
            _imageService = imageService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuProductWithIdDto>>> GetByMenuItem([Id(typeof(MenuItem))] long id)
        {
            var menuProductWithIdDtos = await _menuProductService.GetByMenuItem(id);
            return Ok(menuProductWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuProductWithIdDto>>> GetByCategory([Id(typeof(Category))] long id)
        {
            var menuProductWithIdDtos = await _menuProductService.GetByCategory(id);
            return Ok(menuProductWithIdDtos);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MenuProductWithIdDto>>> GetMany([FromBody] IdsDto ids)
        {
            var menuItemWithIdDtos = await _menuProductService.GetMany(ids);
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

            if (ms.Length > MaxMenuProductImageSizeInMegabytes * 1024 * 1024)
            {
                throw new AkianaException($"Размер изображения превышает максимальный ({MaxMenuProductImageSizeInMegabytes} Мб)");
            }

            ms.Position = 0;

            var imageName = await _imageService.Create(image.FileName, "MenuProduct", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
    }
}