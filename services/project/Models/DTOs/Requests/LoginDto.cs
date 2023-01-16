using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.Requests
{
    public class LoginDto : IDto
    {
        [Required]
        [String(1, 32)]
        public string Login { get; set; }
        
        [Required]
        [String(1, 32)]
        public string Password { get; set; }
    }
}