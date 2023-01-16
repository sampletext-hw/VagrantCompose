using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.WorkerRoles;

namespace Services.SuperuserServices.Abstractions
{
    public interface IWorkerRoleService
    {
        Task<ICollection<WorkerRoleDto>> GetAll();

        Task<GetRolesResultDto> GetByWorker(long id);
        
        Task AddToRole(long workerId, long roleId);

        Task AddToRole(long workerId, string roleTitleEn);

        Task RemoveFromRole(long workerId, long roleId);

        Task RemoveFromRole(long workerId, string roleTitleEn);
    }
}