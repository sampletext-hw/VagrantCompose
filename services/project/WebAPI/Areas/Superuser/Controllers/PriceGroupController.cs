using System;
using System.Threading.Tasks;
using Models.Db.DbRestaurant;
using Models.DTOs.PriceGroups;
using Services.SuperuserServices.Abstractions;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class PriceGroupController : CrudController<PriceGroup, PriceGroupWithIdDto, CreatePriceGroupDto, UpdatePriceGroupDto>
    {
        private readonly IPriceGroupService _priceGroupService;

        public PriceGroupController(IPriceGroupService priceGroupService, Func<Type, object, bool> existenceChecker) : base(priceGroupService, existenceChecker)
        {
            _priceGroupService = priceGroupService;
        }
    }
}