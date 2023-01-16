using System;
using System.Threading.Tasks;
using Models.DTOs.ClientAccounts;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Services.MobileServices.Abstractions
{
    public interface IClientAccountService
    {
        Task<MobileClientLoginRequestDto> LoginOrRegister(MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto);
        Task<MobileClientLoginRequestDto> LoginOrRegisterV2(MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto);

        Task<MobileClientAccountDto> ConfirmCodeAndLogin(MobileConfirmCodeDto mobileConfirmCodeDto);

        Task<MobileClientAccountDto> GetById(long id);

        Task Update(UpdateClientAccountDto updateClientAccountDto);
        Task Delete(long id);
    }
}