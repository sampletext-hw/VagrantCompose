using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.WorkerRoles
{
    public class GetRolesResultDto : IDto
    {
        public ICollection<WorkerRoleDto> Roles { get; set; }

        public GetRolesResultDto(ICollection<WorkerRoleDto> roles)
        {
            Roles = roles;
        }
    }
}