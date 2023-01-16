using Microsoft.Extensions.DependencyInjection;
using Services.ExternalServices.Abstractions;
using Services.ExternalServices.Implementations;

namespace Services.ExternalServices;

public static class DI
{
    public static IServiceCollection AddAkianaExternalServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentCallbackService, PaymentCallbackService>();

        return services;
    }
}