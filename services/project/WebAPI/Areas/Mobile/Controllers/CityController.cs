using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Configs;
using Models.DTOs.Cities;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;
using WebAPI.Utils;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class CityController : AkianaMobileController
    {
        private readonly ICityService _cityService;

        private readonly StaticConfig _staticConfig;
        private readonly IWebHostEnvironment _env;
        
        public CityController(ICityService cityService, IWebHostEnvironment env, IOptions<StaticConfig> staticConfig)
        {
            _cityService = cityService;
            _env = env;
            _staticConfig = staticConfig.Value;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает список всех городов (с шифрацией)")]
        public async Task<ActionResult<ICollection<CityMobileDto>>> GetAll()
        {
            var cityMobileDtos = await _cityService.GetAll();

            return Ok(cityMobileDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Для обеспечения безопасности соединения (с шифрацией)")]
        public ActionResult FilterData()
        {
            var path = Path.GetFullPath(_staticConfig.CertificatePath, _env.ContentRootPath);
            var content = System.IO.File.ReadAllText(path);

            return Ok(content);
        }
    }
}