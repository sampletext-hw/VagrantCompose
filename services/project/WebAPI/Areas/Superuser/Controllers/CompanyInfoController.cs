using System.IO;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Models.DTOs.Misc;
using Models.Misc;
using Services;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class CompanyInfoController : AkianaSuperuserController
    {
        private readonly ICompanyInfoService _companyInfoService;
        private readonly IImageService _imageService;
        
        private const float MaxApplicationStartupImageSizeInMegabytes = 10.0f;

        public CompanyInfoController(ICompanyInfoService companyInfoService, IImageService imageService) : base()
        {
            _companyInfoService = companyInfoService;
            _imageService = imageService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<AboutDataDto>> GetAbout()
        {
            var aboutDataDto = await _companyInfoService.GetAbout();

            return Ok(aboutDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionedEntityDto>> GetDeliveryTerms()
        {
            var versionedContentDataDto = await _companyInfoService.GetDeliveryTerms();

            return Ok(versionedContentDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionedEntityDto>> GetVacancies()
        {
            var versionedContentDataDto = await _companyInfoService.GetVacanciesData();

            return Ok(versionedContentDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ApplicationStartupImageDataDto>> GetApplicationStartupImage()
        {
            var applicationStartupImageData = await _companyInfoService.GetApplicationStartupImageData();

            return Ok(applicationStartupImageData);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ApplicationTerminationDto>> GetApplicationTermination()
        {
            var applicationTerminationData = await _companyInfoService.GetApplicationTerminationData();

            return Ok(applicationTerminationData);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VkUrlDataDto>> GetVkUrl()
        {
            var vkUrlDataDto = await _companyInfoService.GetVkUrlData();

            return Ok(vkUrlDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<InstagramUrlDataDto>> GetInstagramUrl()
        {
            var instagramUrlDataDto = await _companyInfoService.GetInstagramUrlData();

            return Ok(instagramUrlDataDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateAbout([FromBody] UpdateAboutDataDto updateAboutDataDto)
        {
            var versionDto = await _companyInfoService.UpdateAbout(updateAboutDataDto);

            return Ok(versionDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateDeliveryTerms([FromBody] UpdateDeliveryTermsDataDto updateDeliveryTermsDataDto)
        {
            var versionDto = await _companyInfoService.UpdateDeliveryTerms(updateDeliveryTermsDataDto);

            return Ok(versionDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateVacancies([FromBody] UpdateVacanciesDataDto updateVacanciesDataDto)
        {
            var versionDto = await _companyInfoService.UpdateVacanciesData(updateVacanciesDataDto);

            return Ok(versionDto);
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateApplicationStartupImage([FromBody] UpdateApplicationStartupImageDataDto updateApplicationStartupImageDataDto)
        {
            var versionDto = await _companyInfoService.UpdateApplicationStartupImageData(updateApplicationStartupImageDataDto);

            return Ok(versionDto);
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateApplicationTermination([FromBody] UpdateApplicationTerminationDto updateApplicationTerminationDto)
        {
            var versionDto = await _companyInfoService.UpdateApplicationTerminationData(updateApplicationTerminationDto);

            return Ok(versionDto);
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateVkUrl([FromBody] UpdateVkUrlDataDto updateVkUrlDataDto)
        {
            var versionDto = await _companyInfoService.UpdateVkUrlData(updateVkUrlDataDto);

            return Ok(versionDto);
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<VersionDto>> UpdateInstagramUrl([FromBody] UpdateInstagramUrlDataDto updateInstagramUrlDataDto)
        {
            var versionDto = await _companyInfoService.UpdateInstagramUrlData(updateInstagramUrlDataDto);

            return Ok(versionDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ImageDto>> UploadAboutImage(IFormFile image)
        {
            image.EnsureNotNullHandled("image is missing");

            await using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            if (ms.Length == 0)
            {
                throw new AkianaException("image was empty");
            }

            ms.Position = 0;

            var imageName = await _imageService.Create(image.FileName, "CompanyInfoAbout", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        public async Task<ActionResult<ImageDto>> UploadApplicationStartupImage(IFormFile image)
        {
            image.EnsureNotNullHandled("image is missing");

            await using var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            if (ms.Length == 0)
            {
                throw new AkianaException("image was empty");
            }

            if (ms.Length > MaxApplicationStartupImageSizeInMegabytes * 1024 * 1024)
            {
                throw new AkianaException($"Размер изображения превышает максимальный ({MaxApplicationStartupImageSizeInMegabytes} Мб)");
            }

            ms.Position = 0;

            var imageName = await _imageService.Create(image.FileName, "ApplicationStartup", ms.ToArray());

            // var file = await _baseImageService.Create(filename, "Uploaded", ms.ToArray());

            return new ImageDto(imageName);
        }
    }
}