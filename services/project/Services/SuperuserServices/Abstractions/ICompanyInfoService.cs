using System.Threading.Tasks;
using Models.DTOs.CompanyInfos;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    public interface ICompanyInfoService
    {
        Task<AboutDataDto> GetAbout();
        Task<DeliveryTermsDataDto> GetDeliveryTerms();
        Task<VacanciesDataDto> GetVacanciesData();
        Task<ApplicationStartupImageDataDto> GetApplicationStartupImageData();
        Task<ApplicationTerminationDto> GetApplicationTerminationData();
        Task<VkUrlDataDto> GetVkUrlData();
        Task<InstagramUrlDataDto> GetInstagramUrlData();
        
        Task<VersionDto> UpdateAbout(UpdateAboutDataDto updateAboutDataDto);
        Task<VersionDto> UpdateDeliveryTerms(UpdateDeliveryTermsDataDto updateDeliveryTermsDataDto);
        Task<VersionDto> UpdateVacanciesData(UpdateVacanciesDataDto updateVacanciesDataDto);
        Task<VersionDto> UpdateApplicationStartupImageData(UpdateApplicationStartupImageDataDto updateApplicationStartupImageDataDto);
        Task<VersionDto> UpdateApplicationTerminationData(UpdateApplicationTerminationDto updateApplicationTerminationDto);
        Task<VersionDto> UpdateVkUrlData(UpdateVkUrlDataDto updateVkUrlDataDto);
        Task<VersionDto> UpdateInstagramUrlData(UpdateInstagramUrlDataDto updateInstagramUrlDataDto);
    }
}