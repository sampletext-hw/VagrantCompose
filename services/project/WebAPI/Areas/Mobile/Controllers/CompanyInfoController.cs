using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.CompanyInfos;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class CompanyInfoController : AkianaMobileController
    {
        private readonly ICompanyInfoService _companyInfoService;

        public CompanyInfoController(ICompanyInfoService companyInfoService)
        {
            _companyInfoService = companyInfoService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает всю информацию о компании (с шифрацией)")]
        public async Task<ActionResult<MobileAggregatedInfoDto>> GetAll()
        {
            var aggregatedInfoDto = await _companyInfoService.GetAll();

            return Ok(aggregatedInfoDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию \"О компании\" (с шифрацией)")]
        public async Task<ActionResult<MobileAboutDataDto>> GetAbout()
        {
            var aboutDataDto = await _companyInfoService.GetAbout();

            return Ok(aboutDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию \"Условия доставки\" (с шифрацией)")]
        public async Task<ActionResult<MobileDeliveryTermsDataDto>> GetDeliveryTerms()
        {
            var deliveryTermsDataDto = await _companyInfoService.GetDeliveryTerms();

            return Ok(deliveryTermsDataDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию \"Вакансии\" (с шифрацией)")]
        public async Task<ActionResult<MobileVacanciesDataDto>> GetVacancies()
        {
            var vacanciesDataDto = await _companyInfoService.GetVacanciesData();

            return Ok(vacanciesDataDto);
        }
        
        [HttpGet]
        [SwaggerOperation("Получает стартовые картинки приложения (без шифрации)")]
        public async Task<ActionResult<MobileApplicationStartupImageDataDto>> GetApplicationStartupImage()
        {
            var mobileApplicationStartupImageDataDto = await _companyInfoService.GetApplicationStartupImageData();

            return Ok(mobileApplicationStartupImageDataDto);
        }
        
        [HttpGet]
        [SwaggerOperation("Получает информацию о принудительной остановке мобильных приложений (без шифрации)")]
        public async Task<ActionResult<MobileApplicationTerminationDto>> GetApplicationTermination()
        {
            var mobileApplicationTerminationDto = await _companyInfoService.GetApplicationTermination();

            return Ok(mobileApplicationTerminationDto);
        }
        
        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию \"Ссылка ВКонтакте\" (с шифрацией)")]
        public async Task<ActionResult<MobileVkUrlDataDto>> GetVkUrl()
        {
            var vkUrlDataDto = await _companyInfoService.GetVkUrl();

            return Ok(vkUrlDataDto);
        }
        
        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию \"Ссылка Telegram\" (с шифрацией)")]
        public async Task<ActionResult<MobileInstagramUrlDataDto>> GetInstagramUrl()
        {
            var instagramUrlDataDto = await _companyInfoService.GetInstagramUrl();

            return Ok(instagramUrlDataDto);
        }
    }
}