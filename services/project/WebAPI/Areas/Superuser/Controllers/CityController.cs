using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Cities;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    using TBase = City;
    using TWithIdDto = CityWithIdDto;
    using TCreateDto = CreateCityDto;
    using TUpdateDto = UpdateCityDto;

    public class CityController : CrudController<TBase, TWithIdDto, TCreateDto, TUpdateDto>
    {
        private readonly ICityService _service;

        public CityController(ICityService service, Func<Type, object, bool> existenceChecker) : base(service, existenceChecker)
        {
            _service = service;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<TWithIdDto>>> GetByPriceGroup([Id(typeof(PriceGroup))] long id)
        {
            var withIdDtos = await _service.GetByPriceGroup(id);
            return Ok(withIdDtos);
        }
    }
}