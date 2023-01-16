using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.WorkerRoles;
using WebAPI.Filters;
using IWorkerRoleService = Services.SuperuserServices.Abstractions.IWorkerRoleService;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class WorkerRoleController : AkianaSuperuserController
    {
        private readonly IWorkerRoleService _workerRoleService;

        public WorkerRoleController(IWorkerRoleService workerRoleService)
        {
            _workerRoleService = workerRoleService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<WorkerRoleDto>>> GetAll()
        {
            var workerRoleDtos = await _workerRoleService.GetAll();
            return Ok(workerRoleDtos);
        }
    }
}