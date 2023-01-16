using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.ClientAccounts;
using Services.SuperuserServices.Abstractions;
using WebAPI.Filters;

namespace WebAPI.Areas.CallCenter.Controllers
{
    public class ClientAccountController : AkianaCallCenterController
    {
        private readonly IClientAccountService _clientAccountService;

        public ClientAccountController(IClientAccountService clientAccountService)
        {
            _clientAccountService = clientAccountService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.CallCenter)]
        public async Task<ActionResult<ClientAccountDto>> GetById([Id(typeof(ClientAccount))] long id)
        {
            var clientAccountDto = await _clientAccountService.GetById(id);
            return Ok(clientAccountDto);
        }
    }
}