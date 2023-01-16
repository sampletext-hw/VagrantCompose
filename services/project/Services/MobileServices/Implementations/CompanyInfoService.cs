using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Abstractions.CompanyInfo;
using Models.DTOs.CompanyInfos;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class CompanyInfoService : ICompanyInfoService
    {
        private readonly IAboutDataRepository _aboutDataRepository;
        private readonly IDeliveryTermsDataRepository _deliveryTermsDataRepository;
        private readonly IVacanciesDataRepository _vacanciesDataRepository;
        private readonly IApplicationStartupImageDataRepository _applicationStartupImageDataRepository;
        private readonly IApplicationTerminationRepository _applicationTerminationRepository;
        private readonly IVkUrlDataRepository _vkUrlDataRepository;
        private readonly IInstagramUrlDataRepository _instagramUrlDataRepository;


        private readonly IMapper _mapper;

        public CompanyInfoService(IAboutDataRepository aboutDataRepository, IDeliveryTermsDataRepository deliveryTermsDataRepository, IVacanciesDataRepository vacanciesDataRepository, IMapper mapper, IApplicationStartupImageDataRepository applicationStartupImageDataRepository, IApplicationTerminationRepository applicationTerminationRepository, IVkUrlDataRepository vkUrlDataRepository, IInstagramUrlDataRepository instagramUrlDataRepository)
        {
            _aboutDataRepository = aboutDataRepository;
            _deliveryTermsDataRepository = deliveryTermsDataRepository;
            _vacanciesDataRepository = vacanciesDataRepository;
            _mapper = mapper;
            _applicationStartupImageDataRepository = applicationStartupImageDataRepository;
            _applicationTerminationRepository = applicationTerminationRepository;
            _vkUrlDataRepository = vkUrlDataRepository;
            _instagramUrlDataRepository = instagramUrlDataRepository;
        }

        public async Task<MobileAboutDataDto> GetAbout()
        {
            var aboutData = await _aboutDataRepository.GetLastVersionNonTracking();

            var mobileAboutDataDto = _mapper.Map<MobileAboutDataDto>(aboutData);

            return mobileAboutDataDto;
        }

        public async Task<MobileDeliveryTermsDataDto> GetDeliveryTerms()
        {
            var deliveryTermsData = await _deliveryTermsDataRepository.GetLastVersionNonTracking();

            var deliveryTermsDto = _mapper.Map<MobileDeliveryTermsDataDto>(deliveryTermsData);

            return deliveryTermsDto;
        }

        public async Task<MobileVacanciesDataDto> GetVacanciesData()
        {
            var vacanciesData = await _vacanciesDataRepository.GetLastVersionNonTracking();

            var vacanciesDto = _mapper.Map<MobileVacanciesDataDto>(vacanciesData);

            return vacanciesDto;
        }
        
        public async Task<MobileApplicationStartupImageDataDto> GetApplicationStartupImageData()
        {
            var startupImageData = await _applicationStartupImageDataRepository.GetLastVersionNonTracking();

            var startupImageDataDto = _mapper.Map<MobileApplicationStartupImageDataDto>(startupImageData);

            return startupImageDataDto;
        }

        public async Task<MobileApplicationTerminationDto> GetApplicationTermination()
        {
            var applicationTerminationData = await _applicationTerminationRepository.GetLastVersionNonTracking();

            var applicationTerminationDto = _mapper.Map<MobileApplicationTerminationDto>(applicationTerminationData);

            return applicationTerminationDto;
        }

        public async Task<MobileVkUrlDataDto> GetVkUrl()
        {
            var vkUrlData = await _vkUrlDataRepository.GetLastVersionNonTracking();

            var vkUrlDataDto = _mapper.Map<MobileVkUrlDataDto>(vkUrlData);

            return vkUrlDataDto;
        }

        public async Task<MobileInstagramUrlDataDto> GetInstagramUrl()
        {
            var instagramUrlData = await _instagramUrlDataRepository.GetLastVersionNonTracking();

            var instagramUrlDataDto = _mapper.Map<MobileInstagramUrlDataDto>(instagramUrlData);

            return instagramUrlDataDto;
        }

        public async Task<MobileAggregatedInfoDto> GetAll()
        {
            var about = await GetAbout();
            var deliveryTerms = await GetDeliveryTerms();
            var vacanciesData = await GetVacanciesData();
            var vkUrlData = await GetVkUrl();
            var instagramUrlData = await GetInstagramUrl();

            return new MobileAggregatedInfoDto
            {
                About = about,
                DeliveryTerms = deliveryTerms,
                Vacancies = vacanciesData,
                VkUrl = vkUrlData,
                InstagramUrl = instagramUrlData
            };
        }
    }
}