using System;
using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class ClientAccountDto : IDto
    {
        public long Id { get; set; }

        public string Login { get; set; }
        
        public string Username { get; set; }

        public DateTime BirthDate { get; set; }
    }
}