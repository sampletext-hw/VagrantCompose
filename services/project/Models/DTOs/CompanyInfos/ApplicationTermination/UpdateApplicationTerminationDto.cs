using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.ApplicationTermination
{
    public class UpdateApplicationTerminationDto : IDto
    {
        // No [Required], because it forces only 'true' value to be present
        public bool Terminated { get; set; }
    }
}