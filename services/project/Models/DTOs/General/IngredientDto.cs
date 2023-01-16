using Models.DTOs.Misc;

namespace Models.DTOs.General
{
    public class IngredientDto : IDto
    {
        public string Title { get; set; }
        public int Weight { get; set; }
    }
}