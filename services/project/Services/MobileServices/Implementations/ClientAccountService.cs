using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Configs;
using Models.Db;
using Models.Db.Account;
using Models.DTOs.ClientAccounts;
using Models.DTOs.Misc;
using Models.Misc;
using Newtonsoft.Json;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IClientAccountRepository _clientAccountRepository;

        private readonly IClientLoginRequestRepository _clientLoginRequestRepository;

        private readonly ISMSService _smsService;

        private readonly IDeliveryAddressRepository _deliveryAddressRepository;

        private readonly IMapper _mapper;
        private readonly ClientAccountServiceConfig _clientAccountServiceConfig;

        private ILogger<ClientAccountService> _logger;


        public ClientAccountService(IClientAccountRepository clientAccountRepository, IClientLoginRequestRepository clientLoginRequestRepository, ISMSService smsService, IDeliveryAddressRepository deliveryAddressRepository, IMapper mapper, IOptions<ClientAccountServiceConfig> options, ILogger<ClientAccountService> logger)
        {
            _clientAccountRepository = clientAccountRepository;
            _clientLoginRequestRepository = clientLoginRequestRepository;
            _smsService = smsService;
            _deliveryAddressRepository = deliveryAddressRepository;
            _mapper = mapper;
            _logger = logger;
            _clientAccountServiceConfig = options.Value;
        }

        private async Task<(CodeSendType, int)> SendCode(string phone)
        {
            var (result, code) = await _smsService.SendCall(phone);
            if (result)
            {
                _logger.LogInformation("Executed Call Result {phone} {code}", phone, code);
                await TelegramAPI.Send($"Executed Call Result {code}");
                // SMSC возвращает 6 цифр, а нам нужно 4, так что берём модуль
                code %= 10000;
                return (CodeSendType.Call, code);
            }
            else
            {
                code = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
                await _smsService.Send($"Ваш код для входа в приложение Акиана: {code}", phone);
                
                _logger.LogInformation("Sent Sms {phone} {code}", phone, code);
                return (CodeSendType.Sms, code);
            }
        }

        private async Task<MobileClientLoginRequestDto> LoginOrRegisterInternal(MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto, bool smsOnly)
        {
            using var _ = _logger.BeginScope("LoginOrRegisterInternal {request} {sms_only}", JsonConvert.SerializeObject(mobileClientLoginOrRegisterDto), smsOnly);
            // Here we ensure, that client login is in format +79998887766

            // await TelegramAPI.Send($"Login attempt start: {mobileClientLoginOrRegisterDto.Login}, \nSource: {(isAndroid ? "Android" : isIPhone ? "IPhone" : $"Unknown ({requestAccountId})")}");

            var login = mobileClientLoginOrRegisterDto
                .Login
                .Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace(" ", "");

            if (!login.StartsWith('+'))
            {
                login = "+" + login;
            }

            if (login.StartsWith("+8"))
            {
                login = "+7" + login.Substring(2);
            }

            if (!Regex.IsMatch(login, @"^((\+7)(9)+([0-9]){9})$"))
            {
                _logger.LogInformation("Phone was not recognised {login}", login);
                await TelegramAPI.Send($"Phone was not recognized: \"{login}\"");
                throw new AkianaException("Номер телефона не распознан");
            }

            if (await _clientLoginRequestRepository.GetOne(r => r.UniqueId == mobileClientLoginOrRegisterDto.UniqueId) != null)
            {
                _logger.LogError("UniqueId Constraint Violation {unique_id}", mobileClientLoginOrRegisterDto.UniqueId);
                await TelegramAPI.Send($"ClientAccount.LoginOrRegister: Нарушена идентичность запроса (UniqueId): \"{login}\", {mobileClientLoginOrRegisterDto.UniqueId}");
                throw new AkianaException("Нарушена идентичность запроса (UniqueId)");
            }

            var clientAccount = await _clientAccountRepository.GetOne(c => c.Login == login);

            if (clientAccount == null)
            {
                // Client Account Is Non-existent, So Create One

                clientAccount = new ClientAccount
                {
                    Login = mobileClientLoginOrRegisterDto.Login
                };
                await _clientAccountRepository.Add(clientAccount);
            }

            // Here Client Account Is Already Present

            string[] ignoredLogins = _clientAccountServiceConfig.QuotaIgnoredLogins;

            if (!ignoredLogins.Contains(login))
            {
                var requestsForLastDay = await _clientLoginRequestRepository.Count(
                    lr => lr.IssuedAt > DateTime.Now.AddDays(-1) && lr.ClientAccountId == clientAccount.Id && !lr.IsResolved
                );

                if (requestsForLastDay >= _clientAccountServiceConfig.MaxInvalidRequestsPerDay)
                {
                    _logger.LogError("MaxInvalidRequestsPerDay Violation {login}", login);
                    throw new AkianaException("Исчерпан лимит попыток входа. Попробуйте снова через 24 часа.");
                }
            }
            else
            {
                _logger.LogInformation("Quota Ignored for {login}", login);
                await TelegramAPI.Send($"Проигнорировано ограничение на попытки входа для {login}!");
            }
            
            var codeSendType = CodeSendType.Telegram;
            int code;

            if (_clientAccountServiceConfig.UseFakeCode)
            {
                _logger.LogInformation("Used Fake Code for {login}", login);
                code = 5555;
                codeSendType = CodeSendType.Call;
            }
            else
            {
                if (smsOnly)
                {
                    code = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
                    await _smsService.Send($"Ваш код для входа в приложение Акиана: {code}", login);
                }
                else
                {

                    switch (_clientAccountServiceConfig.SendType)
                    {
                        case "Telegram":
                            code = new Random(DateTime.Now.Millisecond).Next(1000, 10000);
                            await TelegramAPI.Send($"{login}, Ваш код для входа в приложение Акиана: {code}");
                            break;
                        case "SMS":
                        case "Call":
                            (codeSendType, code) = await SendCode(login);
                            break;
                        default:
                            throw new("Нераспознана конфигурация отправки!");
                    }
                }
            }

            var clientLoginRequest = new ClientLoginRequest()
            {
                ClientAccountId = clientAccount.Id,
                Code = (uint) code,
                IssuedAt = DateTime.Now,
                IsResolved = false,
                InvalidAttempts = 0,
                UniqueId = mobileClientLoginOrRegisterDto.UniqueId
            };

            await _clientLoginRequestRepository.Add(clientLoginRequest);

            return new MobileClientLoginRequestDto
            {
                Id = clientLoginRequest.Id,
                SendType = codeSendType
            };
        }

        public async Task<MobileClientLoginRequestDto> LoginOrRegister(MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto)
        {
            return await LoginOrRegisterInternal(mobileClientLoginOrRegisterDto, true);
        }

        public async Task<MobileClientLoginRequestDto> LoginOrRegisterV2(MobileClientLoginOrRegisterDto mobileClientLoginOrRegisterDto)
        {
            return await LoginOrRegisterInternal(mobileClientLoginOrRegisterDto, false);
        }

        public async Task<MobileClientAccountDto> ConfirmCodeAndLogin(MobileConfirmCodeDto mobileConfirmCodeDto)
        {
            var clientLoginRequest = await _clientLoginRequestRepository.GetById(mobileConfirmCodeDto.Id);

            if (clientLoginRequest.InvalidAttempts >= _clientAccountServiceConfig.MaxInvalidAttemptsPerDay)
            {
                _logger.LogError("MaxInvalidAttemptsPerDay Violation {client_id}", clientLoginRequest.ClientAccountId);
                throw new AkianaException("Вы ввели неверный код слишком много раз.");
            }

            if (clientLoginRequest.Code != mobileConfirmCodeDto.Code)
            {
                clientLoginRequest.InvalidAttempts++;
                await _clientLoginRequestRepository.Update(clientLoginRequest);
                _logger.LogInformation("Invalid Code entered {client_id} {needed} {entered}", clientLoginRequest.ClientAccountId, clientLoginRequest.Code, mobileConfirmCodeDto.Code);
                throw new AkianaException("Код неверен");
            }

            clientLoginRequest.IsResolved = true;

            await _clientLoginRequestRepository.Update(clientLoginRequest);

            return await GetById(clientLoginRequest.ClientAccountId);
        }

        public async Task<MobileClientAccountDto> GetById(long id)
        {
            var clientAccount = await _clientAccountRepository.GetByIdNonTracking(id,
                a => a.DeliveryAddresses
            );

            // Here we load clientAccount.Addresses Manually

            var addressIds = clientAccount.DeliveryAddresses.Select(a => a.Id);

            clientAccount.DeliveryAddresses = await _deliveryAddressRepository.GetManyNonTracking(
                a => addressIds.Contains(a.Id),
                a => a.LatLng
            );

            var mobileClientAccountDto = _mapper.Map<MobileClientAccountDto>(clientAccount);

            return mobileClientAccountDto;
        }

        public async Task Update(UpdateClientAccountDto updateClientAccountDto)
        {
            var clientAccount = await _clientAccountRepository.GetById(updateClientAccountDto.Id);

            clientAccount.BirthDate = updateClientAccountDto.BirthDate;
            clientAccount.Username = updateClientAccountDto.Username;

            await _clientAccountRepository.Update(clientAccount);
        }

        public async Task Delete(long id)
        {
            var clientAccount = await _clientAccountRepository.GetById(id);

            if (clientAccount is null)
            {
                throw new AkianaException("Аккаунт не найден");
            }

            await _clientAccountRepository.Remove(clientAccount);
        }
    }
}