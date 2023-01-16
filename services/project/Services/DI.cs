using System;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Abstractions;
using Infrastructure.Abstractions.CompanyInfo;
using Infrastructure.Implementations;
using Infrastructure.Implementations.CompanyInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.SuperuserServices;
using Services.AutoMapperProfiles;
using Services.CallCenterServices;
using Services.CommonServices;
using Services.ExternalServices;
using Services.FranchiseeServices;
using Services.MobileServices;
using Services.Shared;

namespace Services
{
    public static class DI
    {
        public static IServiceCollection AddAkianaDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext will take connection string from Environment or throw
            services.AddDbContext<AkianaDbContext>(options => options.UseNpgsql(
                    configuration.GetConnectionString("MyPgDatabase")
                )
            );

            services.AddAkianaRepositories();

            services.AddAkianaFranchiseeServices();
            
            services.AddAkianaSharedServices();

            services.AddAkianaSuperuserServices();

            services.AddAkianaCallCenterServices();

            services.AddAkianaMobileServices();

            services.AddAkianaCommonServices();

            services.AddAkianaExternalServices();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AkianaAutomapperProfile>();
            });

            services.AddScoped<Func<Type, object, bool>>(x =>
            {
                bool Func(Type t, object o)
                {
                    var dbContext = x.GetRequiredService<AkianaDbContext>();
                    return dbContext.Find(t, o) != null;
                }

                return Func;
            });

            return services;
        }

        public static IServiceCollection AddAkianaRepositories(this IServiceCollection services)
        {
            // Add Repositories
            services.AddScoped<IAboutDataRepository, AboutDataRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IClientAccountRepository, ClientAccountRepository>();
            services.AddScoped<IDeliveryTermsDataRepository, DeliveryTermsDataRepository>();
            services.AddScoped<IDeliveryZoneLatLngRepository, DeliveryZoneLatLngRepository>();
            services.AddScoped<IMenuCPFCRepository, MenuCPFCRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IMenuProductRepository, MenuProductRepository>();
            services.AddScoped<IMobilePushByCityRepository, MobilePushByCityRepository>();
            services.AddScoped<IMobilePushByPriceGroupRepository, MobilePushByPriceGroupRepository>();
            services.AddScoped<IOnlinePaymentRepository, OnlinePaymentRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPriceGroupRepository, PriceGroupRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<ITokenSessionRepository, TokenSessionRepository>();
            services.AddScoped<IVacanciesDataRepository, VacanciesDataRepository>();
            services.AddScoped<IWorkerAccountRepository, WorkerAccountRepository>();
            services.AddScoped<IWorkerRoleRepository, WorkerRoleRepository>();
            services.AddScoped<IDeliveryAddressRepository, DeliveryAddressRepository>();
            services.AddScoped<IClientLoginRequestRepository, ClientLoginRequestRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IFavoriteItemRepository, FavoriteItemRepository>();
            services.AddScoped<IRestaurantPickupStopRepository, RestaurantPickupStopRepository>();
            services.AddScoped<IRestaurantDeliveryStopRepository, RestaurantDeliveryStopRepository>();
            services.AddScoped<IApplicationStartupImageDataRepository, ApplicationStartupImageDataRepository>();
            services.AddScoped<IApplicationTerminationRepository, ApplicationTerminationRepository>();
            services.AddScoped<IVkUrlDataRepository, VkUrlDataRepository>();
            services.AddScoped<IInstagramUrlDataRepository, InstagramUrlDataRepository>();

            return services;
        }
    }
}