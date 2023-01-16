using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.MobilePushes;
using Models.DTOs.Misc;
using Models.DTOs.MobilePushes;
using Models.Misc;
using Services;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class MobilePushController : AkianaSuperuserController
    {
        private readonly IMobilePushService _mobilePushService;
        private readonly IImageService _imageService;
        private const float MaxMobilePushImageSizeInMegabytes = 0.5f;

        public MobilePushController(IMobilePushService mobilePushService, IImageService imageService) : base()
        {
            _mobilePushService = mobilePushService;
            _imageService = imageService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MobilePushWithIdDto>>> GetAllByCities()
        {
            var mobilePushWithIdDtos = await _mobilePushService.GetAllByCities();

            return Ok(mobilePushWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MobilePushWithIdDto>>> GetAllByPriceGroups()
        {
            var mobilePushWithIdDtos = await _mobilePushService.GetAllByPriceGroups();

            return Ok(mobilePushWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MobilePushWithIdDto>>> GetByIdByCities([Id(typeof(MobilePushByCity))] long id)
        {
            var mobilePushWithIdDto = await _mobilePushService.GetByIdByCities(id);

            return Ok(mobilePushWithIdDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<MobilePushWithIdDto>>> GetByIdByPriceGroups([Id(typeof(MobilePushByPriceGroup))] long id)
        {
            var mobilePushWithIdDto = await _mobilePushService.GetByIdByPriceGroups(id);

            return Ok(mobilePushWithIdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> CreateByCities([FromBody] CreateMobilePushDto createMobilePushDto)
        {
            var createdDto = await _mobilePushService.CreateByCities(createMobilePushDto);

            return Ok(createdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> CreateByPriceGroups([FromBody] CreateMobilePushDto createMobilePushDto)
        {
            var createdDto = await _mobilePushService.CreateByPriceGroups(createMobilePushDto);

            return Ok(createdDto);
        }
        
        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> DeleteByCities([Id(typeof(MobilePushByCity))] long id)
        {
            await _mobilePushService.DeleteByCities(id);

            return Ok();
        }
        
        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> DeleteByPriceGroups([Id(typeof(MobilePushByPriceGroup))] long id)
        {
            await _mobilePushService.DeleteByPriceGroups(id);

            return Ok();
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

            if (ms.Length > MaxMobilePushImageSizeInMegabytes * 1024 * 1024)
            {
                throw new AkianaException($"Размер изображения превышает максимальный ({MaxMobilePushImageSizeInMegabytes} Мб)");
            }

            ms.Position = 0;

            var imageName = await _imageService.Create(image.FileName, "MobilePush", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
    }
}