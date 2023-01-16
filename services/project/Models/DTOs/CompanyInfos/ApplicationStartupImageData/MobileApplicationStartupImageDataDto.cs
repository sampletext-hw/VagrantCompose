using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.ApplicationStartupImageData
{
    public class MobileApplicationStartupImageDataDto : IDto
    {
        public string BackgroundImage { get; set; }

        public string ForegroundImage { get; set; }
    }
}