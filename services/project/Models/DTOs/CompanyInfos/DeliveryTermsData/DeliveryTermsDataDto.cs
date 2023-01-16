using Models.DTOs.CompanyInfos.Common;

namespace Models.DTOs.CompanyInfos.DeliveryTermsData
{
    public class DeliveryTermsDataDto : VersionedEntityDto
    {
        public string Content { get; set; }
    }
}