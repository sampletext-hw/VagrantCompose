using System.Threading.Tasks;

namespace Services.CommonServices.Abstractions;

public interface IMobileIdStorageService
{
    Task Load();
    
    bool IsAndroidId(long workerAccountId);
    bool IsIosId(long workerAccountId);
}