using System.Threading.Tasks;
using Models.DTOs.CompanyInfos;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;

namespace Services.MobileServices.Abstractions
{
    public interface ICompanyInfoService
    {
        Task<MobileAboutDataDto> GetAbout();
        Task<MobileDeliveryTermsDataDto> GetDeliveryTerms();
        Task<MobileVacanciesDataDto> GetVacanciesData();
        Task<MobileApplicationStartupImageDataDto> GetApplicationStartupImageData();
        Task<MobileApplicationTerminationDto> GetApplicationTermination();
        Task<MobileVkUrlDataDto> GetVkUrl();
        Task<MobileInstagramUrlDataDto> GetInstagramUrl();

        Task<MobileAggregatedInfoDto> GetAll();
    }
}