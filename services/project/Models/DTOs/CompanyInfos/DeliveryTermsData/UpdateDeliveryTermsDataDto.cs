using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.DeliveryTermsData
{
    public class UpdateDeliveryTermsDataDto : IDto
    {
        [Required]
        [String(1, 4096)]
        public string Content { get; set; }
    }
}