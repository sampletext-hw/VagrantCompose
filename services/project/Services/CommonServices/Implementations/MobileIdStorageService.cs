using System;
using System.Threading.Tasks;
using Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Services.CommonServices.Abstractions;

namespace Services.CommonServices.Implementations;

public class MobileIdStorageService : IMobileIdStorageService
{
    private readonly IServiceProvider _provider;
    private long _androidAccountId;
    private long _iPhoneAccountId;

    public MobileIdStorageService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task Load()
    {
        using var scope = _provider.CreateScope();

        var workerAccountRepository = scope.ServiceProvider.GetRequiredService<IWorkerAccountRepository>();

        var androidAccount = await workerAccountRepository.GetOne(w => w.Login == "ANDROID_APPLICATION_v2");
        _androidAccountId = androidAccount.Id;

        var iphoneAccount = await workerAccountRepository.GetOne(w => w.Login == "IPHONE_APPLICATION_v2");
        _iPhoneAccountId = iphoneAccount.Id;
    }

    public bool IsAndroidId(long workerAccountId)
    {
        return workerAccountId == _androidAccountId;
    }

    public bool IsIosId(long workerAccountId)
    {
        return workerAccountId == _iPhoneAccountId;
    }
}