using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Account;
using Models.Db.DbRestaurant;
using Models.DTOs.WorkerAccountDtos;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class WorkerAccountController : CrudController<WorkerAccount, WorkerAccountWithIdDto, CreateWorkerAccountDto, UpdateWorkerAccountDto>
    {
        private readonly IWorkerAccountService _workerAccountService;

        public WorkerAccountController(IWorkerAccountService workerAccountService, Func<Type, object, bool> existenceChecker) : base(workerAccountService, existenceChecker)
        {
            _workerAccountService = workerAccountService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<WorkerAccountWithIdDto>>> GetByRestaurant([Id(typeof(Restaurant))] long id)
        {
            var workerAccountWithIdDtos = await _workerAccountService.GetByRestaurant(id);
            return Ok(workerAccountWithIdDtos);
        }
    }
}
