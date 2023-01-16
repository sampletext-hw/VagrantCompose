using Microsoft.Extensions.DependencyInjection;
using Services.CallCenterServices.Abstractions;
using Services.CallCenterServices.Implementations;

namespace Services.CallCenterServices
{
    public static class DI
    {
        public static IServiceCollection AddAkianaCallCenterServices(this IServiceCollection services)
        {
            // Add Services
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuProductService, MenuProductService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}