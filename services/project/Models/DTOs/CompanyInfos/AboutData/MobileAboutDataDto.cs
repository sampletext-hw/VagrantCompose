using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.AboutData
{
    public class MobileAboutDataDto : IDto
    {
        public string Image { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
    }
}