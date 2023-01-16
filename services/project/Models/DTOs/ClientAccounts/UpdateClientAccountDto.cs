using System;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Account;
using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class UpdateClientAccountDto : IDto
    {
        [Required]
        [Id(typeof(ClientAccount))]
        public long Id { get; set; }

        public DateTime? BirthDate { get; set; }

        [MaxLength(32)]
        public string Username { get; set; }
    }
}