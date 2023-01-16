using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.MobilePushes
{
    public class CreateMobilePushDto : IDto
    {
        [Required]
        [String(1, 42)]
        public string Title { get; set; }

        [Required]
        [String(1, 200)]
        public string Content { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 40)]
        public string Image { get; set; }
        
        [Required]
        public ICollection<IdDto> Targets { get; set; }
    }
}