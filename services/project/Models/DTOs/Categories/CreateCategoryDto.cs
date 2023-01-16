using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.Categories
{
    public class CreateCategoryDto : IDto
    {
        [Required]
        [String(1, 32)]
        public string Title { get; set; }

        public bool IsDeletable { get; set; } = true;
    }
}