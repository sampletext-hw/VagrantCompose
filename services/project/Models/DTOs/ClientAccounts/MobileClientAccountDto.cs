using System;
using System.Collections.Generic;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class MobileClientAccountDto : IDto
    {
        public long Id { get; set; }

        public string Login { get; set; }
        
        public string Username { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<DeliveryAddressMobileDto> DeliveryAddresses { get; set; }
    }
}