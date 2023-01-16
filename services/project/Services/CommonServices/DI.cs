using Microsoft.Extensions.DependencyInjection;
using Services.CommonServices.Abstractions;
using Services.CommonServices.Implementations;

namespace Services.CommonServices
{
    public static class DI
    {
        public static IServiceCollection AddAkianaCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IRequestCounterService, RequestCounterService>();
            services.AddSingleton<IFCMService, FCMService>();
            services.AddSingleton<ISMSService, SMSService>();
            services.AddSingleton<ITokenRoleCacheService, TokenRoleCacheService>();
            services.AddSingleton<IRestaurantByCityCacheService, RestaurantByCityCacheService>();
            services.AddSingleton<ISSEService, SSEService>();
            services.AddSingleton<IEmailService, EmailService>();

            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<IOrderPostProcessingQueue, OrderPostProcessingQueue>();
            services.AddSingleton<IMobileIdStorageService, MobileIdStorageService>();
            services.AddSingleton<IOrderPostProcessService, OrderPostProcessService>();

            return services;
        }
    }
}