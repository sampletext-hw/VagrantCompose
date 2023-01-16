using System;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Abstractions.CompanyInfo;
using Models.Db.CompanyInfo;
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
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
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

        public async Task<AboutDataDto> GetAbout()
        {
            var aboutData = await _aboutDataRepository.GetLastVersionNonTracking();

            var versionedContentDataDto = _mapper.Map<AboutDataDto>(aboutData);

            return versionedContentDataDto;
        }

        public async Task<DeliveryTermsDataDto> GetDeliveryTerms()
        {
            var deliveryTermsData = await _deliveryTermsDataRepository.GetLastVersionNonTracking();

            var deliveryTermsDataDto = _mapper.Map<DeliveryTermsDataDto>(deliveryTermsData);

            return deliveryTermsDataDto;
        }

        public async Task<VacanciesDataDto> GetVacanciesData()
        {
            var vacanciesData = await _vacanciesDataRepository.GetLastVersionNonTracking();

            var vacanciesDataDto = _mapper.Map<VacanciesDataDto>(vacanciesData);

            return vacanciesDataDto;
        }

        public async Task<ApplicationStartupImageDataDto> GetApplicationStartupImageData()
        {
            var applicationStartupImageData = await _applicationStartupImageDataRepository.GetLastVersionNonTracking();

            var applicationStartupImageDataDto = _mapper.Map<ApplicationStartupImageDataDto>(applicationStartupImageData);

            return applicationStartupImageDataDto;
        }

        public async Task<ApplicationTerminationDto> GetApplicationTerminationData()
        {
            var applicationTerminationData = await _applicationTerminationRepository.GetLastVersionNonTracking();

            var applicationTerminationDto = _mapper.Map<ApplicationTerminationDto>(applicationTerminationData);

            return applicationTerminationDto;
        }

        public async Task<VkUrlDataDto> GetVkUrlData()
        {
            var vkUrlData = await _vkUrlDataRepository.GetLastVersionNonTracking();

            var vkUrlDataDto = _mapper.Map<VkUrlDataDto>(vkUrlData);

            return vkUrlDataDto;
        }

        public async Task<InstagramUrlDataDto> GetInstagramUrlData()
        {
            var instagramUrlData = await _instagramUrlDataRepository.GetLastVersionNonTracking();

            var instagramUrlDataDto = _mapper.Map<InstagramUrlDataDto>(instagramUrlData);

            return instagramUrlDataDto;
        }

        public async Task<VersionDto> UpdateAbout(UpdateAboutDataDto updateAboutDataDto)
        {
            var lastVersion = await _aboutDataRepository.GetLastVersion();

            var aboutData = _mapper.Map<AboutData>(updateAboutDataDto);
            aboutData.DateTime = DateTime.Now;
            aboutData.Version = lastVersion.Version + 1;

            await _aboutDataRepository.Add(aboutData);

            return aboutData.Version;
        }

        public async Task<VersionDto> UpdateDeliveryTerms(UpdateDeliveryTermsDataDto updateDeliveryTermsDataDto)
        {
            var lastVersion = await _deliveryTermsDataRepository.GetLastVersion();

            var deliveryTermsData = _mapper.Map<DeliveryTermsData>(updateDeliveryTermsDataDto);
            deliveryTermsData.DateTime = DateTime.Now;
            deliveryTermsData.Version = lastVersion.Version + 1;

            await _deliveryTermsDataRepository.Add(deliveryTermsData);

            return deliveryTermsData.Version;
        }

        public async Task<VersionDto> UpdateVacanciesData(UpdateVacanciesDataDto updateVacanciesDataDto)
        {
            var lastVersion = await _vacanciesDataRepository.GetLastVersion();

            var vacanciesData = _mapper.Map<VacanciesData>(updateVacanciesDataDto);
            vacanciesData.DateTime = DateTime.Now;
            vacanciesData.Version = lastVersion.Version + 1;

            await _vacanciesDataRepository.Add(vacanciesData);

            return vacanciesData.Version;
        }

        public async Task<VersionDto> UpdateApplicationStartupImageData(UpdateApplicationStartupImageDataDto updateApplicationStartupImageDataDto)
        {
            var lastVersion = await _applicationStartupImageDataRepository.GetLastVersion();

            var mobileStartupImageData = _mapper.Map<ApplicationStartupImageData>(updateApplicationStartupImageDataDto);
            mobileStartupImageData.DateTime = DateTime.Now;
            mobileStartupImageData.Version = lastVersion.Version + 1;

            await _applicationStartupImageDataRepository.Add(mobileStartupImageData);

            return mobileStartupImageData.Version;
        }

        public async Task<VersionDto> UpdateApplicationTerminationData(UpdateApplicationTerminationDto updateApplicationTerminationDto)
        {
            var lastVersion = await _applicationTerminationRepository.GetLastVersion();

            var applicationTerminationData = _mapper.Map<ApplicationTerminationData>(updateApplicationTerminationDto);
            applicationTerminationData.DateTime = DateTime.Now;
            applicationTerminationData.Version = lastVersion.Version + 1;

            await _applicationTerminationRepository.Add(applicationTerminationData);

            return applicationTerminationData.Version;
        }

        public async Task<VersionDto> UpdateVkUrlData(UpdateVkUrlDataDto updateVkUrlDataDto)
        {
            var lastVersion = await _vkUrlDataRepository.GetLastVersion();

            var vkUrlData = _mapper.Map<VkUrlData>(updateVkUrlDataDto);
            vkUrlData.DateTime = DateTime.Now;
            vkUrlData.Version = lastVersion.Version + 1;

            await _vkUrlDataRepository.Add(vkUrlData);

            return vkUrlData.Version;
        }

        public async Task<VersionDto> UpdateInstagramUrlData(UpdateInstagramUrlDataDto updateInstagramUrlDataDto)
        {
            var lastVersion = await _instagramUrlDataRepository.GetLastVersion();

            var instagramUrlData = _mapper.Map<InstagramUrlData>(updateInstagramUrlDataDto);
            instagramUrlData.DateTime = DateTime.Now;
            instagramUrlData.Version = lastVersion.Version + 1;

            await _instagramUrlDataRepository.Add(instagramUrlData);

            return instagramUrlData.Version;
        }
    }
}