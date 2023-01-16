using System;
using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.MobilePushes
{
    public class MobilePushWithIdDto : IDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public ICollection<IdDto> Targets { get; set; }
    }
}