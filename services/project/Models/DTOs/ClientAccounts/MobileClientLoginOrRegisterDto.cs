using System;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class MobileClientLoginOrRegisterDto : IDto
    {
        [Required]
        // Other requirements are done in service
        public string Login { get; set; }

        [Required]
        public Guid UniqueId { get; set; }
    }
}