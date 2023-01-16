using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations
{
    public class TokenRoleCacheService : ITokenRoleCacheService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Mutex _mutex;
        private readonly Dictionary<string, string[]> _roles;

        public TokenRoleCacheService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mutex = new();
            _roles = new();
        }

        public async Task<bool> HasRole(string token, string role)
        {
            if (_roles.ContainsKey(token)) return _roles[token].Contains(role);

            if (!await LoadWorkerRoles(token)) return false;

            return _roles[token].Contains(role);
        }

        public async Task<bool> HasAnyRole(string token, string[] roles)
        {
            if (_roles.ContainsKey(token))
            {
                return _roles[token].Any(roles.Contains);
            }

            if (!await LoadWorkerRoles(token)) return false;

            return _roles[token].Any(roles.Contains);
        }

        private async Task<bool> LoadWorkerRoles(string token)
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var tokenSessionRepository = serviceScope.ServiceProvider.GetRequiredService<ITokenSessionRepository>();
            var workerAccountRepository = serviceScope.ServiceProvider.GetRequiredService<IWorkerAccountRepository>();

            var tokenSession = await tokenSessionRepository.GetByToken(token);

            if (tokenSession == null)
            {
                return false;
            }

            var workerAccount = await workerAccountRepository
                .GetByIdNonTracking(
                    tokenSession.WorkerAccountId,
                    t => t.WorkerRoles
                );

            // WTF, NO ACCOUNT FOR SESSION - EXCEPTION
            workerAccount.EnsureNotNullFatal("TokenRoleStorageService: Worker Account Not Found For Sessions");

            Save(token, workerAccount.WorkerRoles.Select(r => r.TitleEn).ToArray());
            return true;
        }

        public void Save(string token, string[] roles)
        {
            _mutex.WaitOne();
            if (_roles.ContainsKey(token))
            {
                _roles.Remove(token);
            }

            _roles.Add(token, roles);
            _mutex.ReleaseMutex();
        }

        public Task Clear(string token)
        {
            // ReSharper disable once InvertIf
            if (_roles.ContainsKey(token))
            {
                _mutex.WaitOne();
                _roles.Remove(token);
                _mutex.ReleaseMutex();
            }

            return Task.CompletedTask;
        }

        public async Task Clear(long workerId)
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var tokenSessionRepository = serviceScope.ServiceProvider.GetRequiredService<ITokenSessionRepository>();

            var tokenSessions = await tokenSessionRepository.GetActiveByWorkerNonTracking(workerId);

            foreach (var tokenSession in tokenSessions)
            {
                await Clear(tokenSession.Token);
            }
        }
    }
}