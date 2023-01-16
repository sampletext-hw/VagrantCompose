using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.DbRestaurant;
using Models.DTOs.Misc;

namespace Models.DTOs.WorkerAccountDtos
{
    public class CreateWorkerAccountDto : IDto
    {
        [Required]
        [String(1, 32)]
        public string Login { get; set; }

        [Required]
        [String(1, 32)]
        public string Password { get; set; }
        
        [Required(AllowEmptyStrings = true)]
        [String(0, 32)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 32)]
        public string Surname { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 32)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 64)]
        public string Email { get; set; }

        [Required]
        public ICollection<IdDto> Restaurants { get; set; }
        
        [Required]
        public ICollection<IdDto> WorkerRoles { get; set; }

    }
}