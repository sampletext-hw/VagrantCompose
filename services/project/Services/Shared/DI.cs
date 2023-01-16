using Microsoft.Extensions.DependencyInjection;
using Services.Shared.Abstractions;
using Services.Shared.Implementations;

namespace Services.Shared
{
    public static class DI
    {
        public static IServiceCollection AddAkianaSharedServices(this IServiceCollection services)
        {
            services.AddScoped<RequestAccountIdService>();
            services.AddScoped<IRequestAccountIdService, RequestAccountIdService>(x => x.GetRequiredService<RequestAccountIdService>());
            services.AddScoped<IRequestAccountIdSetterService, RequestAccountIdService>(x => x.GetRequiredService<RequestAccountIdService>());
            
            services.AddScoped<ITokenSessionService, TokenSessionService>();
            services.AddScoped<IWorkerRoleService, WorkerRoleService>();

            services.AddMemoryCache();
            services.AddSingleton<IIPCache, IpCache>();
            return services;
        }
    }
}