using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Banners;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class BannerController : AkianaMobileController
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает баннеры по заданному городу (с шифрацией)")]
        public async Task<ActionResult<ICollection<BannerMobileDto>>> GetByCity([Id(typeof(City))] long id)
        {
            var bannerMobileDtos = await _bannerService.GetByCity(id);

            return Ok(bannerMobileDtos);
        }
    }
}