using Models.DTOs.Misc;

namespace Models.DTOs.PriceGroups
{
    public class PriceGroupWithIdDto : IDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
    }
}