using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class RestaurantStopController : AkianaSuperuserController
    {
        private readonly IRestaurantStopService _restaurantStopService;

        public RestaurantStopController(IRestaurantStopService restaurantStopService)
        {
            _restaurantStopService = restaurantStopService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<RestaurantStopDto>>> GetPickupByRestaurant([Id(typeof(Restaurant))] long id)
        {
            var pickupByRestaurant = await _restaurantStopService.GetPickupByRestaurant(id);

            return Ok(pickupByRestaurant);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<RestaurantStopDto>>> GetDeliveryByRestaurant([Id(typeof(Restaurant))] long id)
        {
            var deliveryByRestaurant = await _restaurantStopService.GetDeliveryByRestaurant(id);

            return Ok(deliveryByRestaurant);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> CreatePickupStop([FromBody] CreateRestaurantStopDto createRestaurantStopDto)
        {
            var createdDto = await _restaurantStopService.CreatePickupStop(createRestaurantStopDto);
            return Ok(createdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> CreateDeliveryStop([FromBody] CreateRestaurantStopDto createRestaurantStopDto)
        {
            var createdDto = await _restaurantStopService.CreateDeliveryStop(createRestaurantStopDto);
            return Ok(createdDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult> FinishPickupStop([Id(typeof(Restaurant))] long id)
        {
            await _restaurantStopService.FinishPickupStop(id);
            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult> FinishDeliveryStop([Id(typeof(Restaurant))] long id)
        {
            await _restaurantStopService.FinishDeliveryStop(id);
            return Ok();
        }
    }
}