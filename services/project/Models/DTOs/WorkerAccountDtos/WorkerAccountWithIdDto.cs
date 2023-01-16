using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.WorkerAccountDtos
{
    public class WorkerAccountWithIdDto : IDto
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }
        
        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ICollection<IdDto> Restaurants { get; set; }

        public ICollection<IdDto> WorkerRoles { get; set; }
    }
}