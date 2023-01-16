using Models.DTOs.Misc;

namespace Models.DTOs.Banners
{
    public class BannerMobileDto : IDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string ExtUrl { get; set; }
    }
}
