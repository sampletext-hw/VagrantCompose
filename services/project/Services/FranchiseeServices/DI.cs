using Microsoft.Extensions.DependencyInjection;
using Services.FranchiseeServices.Abstractions;
using Services.FranchiseeServices.Implementations;
using Services.Shared.Abstractions;
using Services.Shared.Implementations;

namespace Services.FranchiseeServices
{
    public static class DI
    {
        public static IServiceCollection AddAkianaFranchiseeServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IRestaurantStopService, RestaurantStopService>();
            return services;
        }
    }
}