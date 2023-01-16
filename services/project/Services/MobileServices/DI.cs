using Microsoft.Extensions.DependencyInjection;
using Services.MobileServices.Abstractions;
using Services.MobileServices.Implementations;

namespace Services.MobileServices
{
    public static class DI
    {
        public static IServiceCollection AddAkianaMobileServices(this IServiceCollection services)
        {
            // Add Services
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICompanyInfoService, CompanyInfoService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IMenuProductService, MenuProductService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IClientAccountService, ClientAccountService>();
            services.AddScoped<IDeliveryAddressService, DeliveryAddressService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            return services;
        }
    }
}