using System.Collections.Generic;

namespace Models.DTOs.Misc
{
    public class IdsDto : IDto
    {
        public ICollection<IdDto> Ids { get; set; }
    }
}