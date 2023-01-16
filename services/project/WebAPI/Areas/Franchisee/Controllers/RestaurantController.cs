using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Restaurants;
using Services.FranchiseeServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Franchisee.Controllers
{
    public class RestaurantController : AkianaFranchiseeController
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult<ICollection<RestaurantWithIdDto>>> GetMy()
        {
            var restaurantWithIdDtos = await _restaurantService.GetMy();

            return Ok(restaurantWithIdDtos);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Franchisee)]
        public async Task<ActionResult<RestaurantWithIdDto>> GetById([Id(typeof(Restaurant))] long id)
        {
            var restaurantWithIdDtos = await _restaurantService.GetById(id);

            return Ok(restaurantWithIdDtos);
        }
    }
}