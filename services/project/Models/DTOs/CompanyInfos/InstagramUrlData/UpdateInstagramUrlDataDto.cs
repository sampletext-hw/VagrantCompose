using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.InstagramUrlData
{
    public class UpdateInstagramUrlDataDto : IDto
    {
        [Required]
        [String(1, 4096)]
        public string Content { get; set; }
    }
}