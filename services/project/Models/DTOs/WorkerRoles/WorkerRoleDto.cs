using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.WorkerRoles
{
    public class WorkerRoleDto : IDto
    {
        public long Id { get; set; }

        public string TitleRu { get; set; }

        public string TitleEn { get; set; }
    }
}