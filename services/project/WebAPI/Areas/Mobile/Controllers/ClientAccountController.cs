using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.ClientAccounts;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class ClientAccountController : AkianaMobileController
    {
        private readonly IClientAccountService _clientAccountService;

        public ClientAccountController(IClientAccountService clientAccountService)
        {
            _clientAccountService = clientAccountService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Создаёт заявку на вход в приложение (отправляет только СМС) (с шифрацией)")]
        public async Task<ActionResult<MobileClientLoginRequestDto>> LoginOrRegister([ModelBinder(typeof(EncodedJsonBinder))] MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto)
        {
            var mobileClientLoginRequestDto = await _clientAccountService.LoginOrRegister(mobileClientLoginOrRegisterDto);
            
            return Ok(mobileClientLoginRequestDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Создаёт заявку на вход в приложение (пробует отправить звонок и откатывает до СМС в случае ошибки) (с шифрацией)")]
        public async Task<ActionResult<MobileClientLoginRequestDto>> LoginOrRegisterV2([ModelBinder(typeof(EncodedJsonBinder))] MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto)
        {
            var mobileClientLoginRequestDto = await _clientAccountService.LoginOrRegisterV2(mobileClientLoginOrRegisterDto);
            
            return Ok(mobileClientLoginRequestDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Принимает код для входа в приложение и возвращает Id клиента (с шифрацией)")]
        public async Task<ActionResult<MobileClientAccountDto>> ConfirmCodeAndEnter([ModelBinder(typeof(EncodedJsonBinder))] MobileConfirmCodeDto mobileConfirmCodeDto)
        {
            var mobileClientAccountDto = await _clientAccountService.ConfirmCodeAndLogin(mobileConfirmCodeDto);

            return Ok(mobileClientAccountDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает информацию о клиенте по Id (с шифрацией)")]
        public async Task<ActionResult<MobileClientAccountDto>> GetById([Id(typeof(ClientAccount))] long id)
        {
            var mobileClientAccountDto = await _clientAccountService.GetById(id);

            return Ok(mobileClientAccountDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [SwaggerOperation("Обновляет информацию о клиенте (без шифрации)")]
        public async Task<ActionResult> Update([ModelBinder(typeof(EncodedJsonBinder))] UpdateClientAccountDto updateClientAccountDto)
        {
            await _clientAccountService.Update(updateClientAccountDto);

            return Ok();
        }

        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [SwaggerOperation("Удаляет клиента (без шифрации)")]
        public async Task<ActionResult> Delete(long id)
        {
            await _clientAccountService.Delete(id);

            return Ok();
        }
    }
}