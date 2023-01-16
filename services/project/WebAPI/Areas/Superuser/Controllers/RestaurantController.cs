using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Restaurants;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class RestaurantController : CrudController<Restaurant, RestaurantWithIdDto, CreateRestaurantDto, UpdateRestaurantDto>
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService, Func<Type, object, bool> existenceChecker) : base(restaurantService, existenceChecker)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<RestaurantWithIdDto>>> GetByCity([Id(typeof(City))] long id)
        {
            var restaurantWithIdDtos = await _restaurantService.GetByCity(id);
            return Ok(restaurantWithIdDtos);
        }
    }
}