using Microsoft.Extensions.DependencyInjection;
using Services.SuperuserServices.Abstractions;
using Services.SuperuserServices.Implementations;

namespace Services.SuperuserServices
{
    public static class DI
    {
        public static IServiceCollection AddAkianaSuperuserServices(this IServiceCollection services)
        {
            // Add Services
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IClientAccountService, ClientAccountService>();
            services.AddScoped<ICompanyInfoService, CompanyInfoService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuProductService, MenuProductService>();
            services.AddScoped<IMobilePushService, MobilePushService>();
            services.AddScoped<IPriceGroupService, PriceGroupService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IWorkerAccountService, WorkerAccountService>();
            services.AddScoped<IWorkerRoleService, WorkerRoleService>();
            services.AddScoped<IDeliveryAddressService, DeliveryAddressService>();
            services.AddScoped<IRestaurantStopService, RestaurantStopService>();

            return services;
        }
    }
}