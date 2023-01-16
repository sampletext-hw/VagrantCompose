using Models.DTOs.CompanyInfos.Common;

namespace Models.DTOs.CompanyInfos.VacanciesData
{
    public class VacanciesDataDto : VersionedEntityDto
    {
        public string Content { get; set; }
    }
}