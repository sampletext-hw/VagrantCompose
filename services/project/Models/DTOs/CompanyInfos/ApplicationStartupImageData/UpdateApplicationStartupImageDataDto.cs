using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.ApplicationStartupImageData
{
    public class UpdateApplicationStartupImageDataDto : IDto
    {
        [Required]
        [String(1, 40)]
        public string BackgroundImage { get; set; }

        [Required]
        [String(1, 40)]
        public string ForegroundImage { get; set; }
    }
}