using System.Collections.Generic;
using Models.DTOs.Misc;

namespace Models.DTOs.Banners
{
    public class BannerWithIdDto : IDto
    {
        public long Id { get; set; }

        public ICollection<IdDto> Cities { get; set; }

        public bool IsActive { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string ExtUrl { get; set; }
    }
}