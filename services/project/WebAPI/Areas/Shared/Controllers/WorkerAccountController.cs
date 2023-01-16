using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Misc;
using Models.DTOs.Requests;
using Models.DTOs.WorkerRoles;
using Models.Misc;
using Newtonsoft.Json;
using Services.ExternalServices;
using Services.Shared.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Shared.Controllers
{
    public class WorkerAccountController : AkianaSharedController
    {
        private readonly ITokenSessionService _tokenSessionService;
        private readonly IWorkerRoleService _workerRoleService;

        private readonly IIPCache _ipCache;

        public WorkerAccountController(ITokenSessionService tokenSessionService, IWorkerRoleService workerRoleService, IIPCache ipCache)
        {
            _tokenSessionService = tokenSessionService;
            _workerRoleService = workerRoleService;
            _ipCache = ipCache;
        }

        [HttpPost]
        [SwaggerOperation("Авторизация пользователя (без шифрации)")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginDto loginDto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var loginResultDto = await _tokenSessionService.Login(loginDto, ip);

            return Ok(loginResultDto);
        }

        [HttpPost]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Авторизация пользователя (с шифрацией)")]
        public async Task<ActionResult<LoginResultDto>> LoginV2([ModelBinder(typeof(EncodedJsonBinder))] LoginDto loginDto)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var loginResultDto = await _tokenSessionService.LoginV2(loginDto, ip);

            return Ok(loginResultDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        public async Task<ActionResult<MessageDto>> Logout()
        {
            ControllerContext.HttpContext.TryGetAuthToken(out var token);

            await _tokenSessionService.Logout(token);

            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        public async Task<ActionResult<GetRolesResultDto>> GetMyRoles()
        {
            return Ok(await _workerRoleService.GetMy());
        }
    }
}