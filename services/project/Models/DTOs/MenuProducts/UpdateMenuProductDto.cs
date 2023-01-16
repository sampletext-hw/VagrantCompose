using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.Db.Common;
using Models.Db.Menu;
using Models.DTOs.Misc;

namespace Models.DTOs.MenuProducts
{
    public class UpdateMenuProductDto : IDto
    {
        [Required]
        [Id(typeof(MenuProduct))]
        public long Id { get; set; }

        [Required]
        [String(1, 48)]
        public string Title { get; set; }

        [Required]
        [String(1, 512)]
        public string Content { get; set; }

        [Required]
        [Id(typeof(Category))]
        public long CategoryId { get; set; }

        [Required]
        [String(1, 40)]
        public string Image { get; set; }
    }
}