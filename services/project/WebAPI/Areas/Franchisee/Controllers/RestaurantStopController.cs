using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;
using Models.DTOs.RestaurantStops;
using Services.FranchiseeServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Franchisee.Controllers
{
    public class RestaurantStopController : AkianaFranchiseeController
    {
        private readonly IRestaurantStopService _restaurantStopService;

        public RestaurantStopController(IRestaurantStopService restaurantStopService)
        {
            _restaurantStopService = restaurantStopService;
        }
        
        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult<CreatedDto>> CreatePickupStop([FromBody] CreateRestaurantStopDto createRestaurantStopDto)
        {
            var createdDto = await _restaurantStopService.CreatePickupStop(createRestaurantStopDto);
            return Ok(createdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult<CreatedDto>> CreateDeliveryStop([FromBody] CreateRestaurantStopDto createRestaurantStopDto)
        {
            var createdDto = await _restaurantStopService.CreateDeliveryStop(createRestaurantStopDto);
            return Ok(createdDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult> FinishPickupStop([Id(typeof(Restaurant))] long id)
        {
            await _restaurantStopService.FinishPickupStop(id);
            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult> FinishDeliveryStop([Id(typeof(Restaurant))] long id)
        {
            await _restaurantStopService.FinishDeliveryStop(id);
            return Ok();
        }
    }
}