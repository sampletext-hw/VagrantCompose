using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.Categories
{
    public class UpdateCategoryDto : IDto
    {
        [Required]
        [Id(typeof(Category))]
        public long Id { get; set; }
        
        [Required]
        [String(1, 32)]
        public string Title { get; set; }
    }
}