using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db;
using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class MobileConfirmCodeDto : IDto
    {
        [Required]
        [Id(typeof(ClientLoginRequest))]
        public long Id { get; set; }

        [Required]
        [Range(0, 9999)]
        public uint Code { get; set; }
    }
}