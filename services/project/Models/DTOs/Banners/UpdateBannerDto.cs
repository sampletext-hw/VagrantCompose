using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db;
using Models.DTOs.Misc;

namespace Models.DTOs.Banners
{
    public class UpdateBannerDto : IDto
    {
        [Required]
        [Id(typeof(Banner))]
        public long Id { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [String(1, 64)]
        public string Title { get; set; }

        [Required]
        [String(1, 1024)]
        public string Description { get; set; }

        [Required]
        [String(1, 40)]
        public string Image { get; set; }

        [Required(AllowEmptyStrings = true)]
        [String(0, 4096)]
        public string ExtUrl { get; set; }

        [Required]
        public ICollection<IdDto> Cities { get; set; }
    }
}