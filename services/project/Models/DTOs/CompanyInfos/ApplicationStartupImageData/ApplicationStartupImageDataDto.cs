using Models.DTOs.CompanyInfos.Common;

namespace Models.DTOs.CompanyInfos.ApplicationStartupImageData
{
    public class ApplicationStartupImageDataDto : VersionedEntityDto
    {
        public string BackgroundImage { get; set; }

        public string ForegroundImage { get; set; }
    }
}