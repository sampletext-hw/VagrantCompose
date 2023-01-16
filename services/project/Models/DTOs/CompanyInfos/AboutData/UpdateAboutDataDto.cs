using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.AboutData
{
    public class UpdateAboutDataDto : IDto
    {
        [Required]
        [String(1, 40)]
        public string Image { get; set; }
        
        [Required]
        [String(1, 128)]
        public string Title { get; set; }
        
        [Required]
        [String(1, 4096)]
        public string Content { get; set; }
    }
}