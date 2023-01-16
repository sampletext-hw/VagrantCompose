using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos
{
    public class MobileAggregatedInfoDto : IDto
    {
        public MobileAboutDataDto About { get; set; }

        public MobileDeliveryTermsDataDto DeliveryTerms { get; set; }

        public MobileVacanciesDataDto Vacancies { get; set; }
        
        public MobileVkUrlDataDto VkUrl { get; set; }
        
        public MobileInstagramUrlDataDto InstagramUrl { get; set; }
    }
}