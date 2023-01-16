using System.Threading.Tasks;
using Models.DTOs.WorkerRoles;

namespace Services.Shared.Abstractions
{
    public interface IWorkerRoleService
    {
        Task<GetRolesResultDto> GetByWorker(long id);

        Task<GetRolesResultDto> GetMy();
    }
}