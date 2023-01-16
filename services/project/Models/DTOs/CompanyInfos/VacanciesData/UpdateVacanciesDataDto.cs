using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.VacanciesData
{
    public class UpdateVacanciesDataDto : IDto
    {
        [Required]
        [String(1, 4096)]
        public string Content { get; set; }
    }
}