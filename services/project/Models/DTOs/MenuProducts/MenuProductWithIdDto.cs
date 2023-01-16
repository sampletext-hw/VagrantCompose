using Models.DTOs.Misc;

namespace Models.DTOs.MenuProducts
{
    public class MenuProductWithIdDto : IDto
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long CategoryId { get; set; }
        public string CategoryTitle { get; set; }

        public string Image { get; set; }
    }
}