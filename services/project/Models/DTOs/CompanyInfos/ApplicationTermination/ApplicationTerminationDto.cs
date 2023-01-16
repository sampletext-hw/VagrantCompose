using Models.DTOs.CompanyInfos.Common;

namespace Models.DTOs.CompanyInfos.ApplicationTermination
{
    public class ApplicationTerminationDto : VersionedEntityDto
    {
        public bool Terminated { get; set; }
    }
}