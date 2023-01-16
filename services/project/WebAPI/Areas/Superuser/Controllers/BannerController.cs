using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db;
using Models.Db.DbRestaurant;
using Models.DTOs.Banners;
using Models.DTOs.Misc;
using Models.Misc;
using Services;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class BannerController : CrudController<Banner, BannerWithIdDto, CreateBannerDto, UpdateBannerDto>
    {
        private readonly IBannerService _bannerService;
        private readonly IImageService _imageService;
        private const float MaxBannerImageSizeInMegabytes = 10;

        public BannerController(IBannerService bannerService, Func<Type, object, bool> existenceChecker, IImageService imageService) : base(bannerService, existenceChecker)
        {
            _bannerService = bannerService;
            _imageService = imageService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<BannerWithIdDto>>> GetByCity([Id(typeof(City))] long id)
        {
            var bannerWithIdDtos = await _bannerService.GetByCity(id);
            return Ok(bannerWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<IsActiveDto>> ToggleActive([Id(typeof(Banner))] long id)
        {
            var isActiveDto = await _bannerService.ToggleActive(id);
            return Ok(isActiveDto);
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

            if (ms.Length > MaxBannerImageSizeInMegabytes * 1024 * 1024)
            {
                throw new AkianaException($"Размер изображения превышает максимальный ({MaxBannerImageSizeInMegabytes} Мб)");
            }

            ms.Position = 0;

            var imageName = await _imageService.Create(image.FileName, "Banner", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
    }
}