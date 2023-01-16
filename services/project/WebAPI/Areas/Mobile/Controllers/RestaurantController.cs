using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Restaurants;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class RestaurantController : AkianaMobileController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает все рестораны 1 города (с шифрацией)")]
        public async Task<ActionResult<ICollection<RestaurantMobileDto>>> GetByCity([Id(typeof(City))] long id)
        {
            var restaurantMobileDtos = await _restaurantService.GetByCity(id);

            return Ok(restaurantMobileDtos);
        }
    }
}