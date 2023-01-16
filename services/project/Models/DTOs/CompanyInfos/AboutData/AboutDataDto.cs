using Models.DTOs.CompanyInfos.Common;

namespace Models.DTOs.CompanyInfos.AboutData
{
    public class AboutDataDto : VersionedEntityDto
    {
        public string Image { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
    }
}