using Models.DTOs.Misc;

namespace Models.DTOs.Categories
{
    public class CategoryWithIdDto : IDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
    }
}