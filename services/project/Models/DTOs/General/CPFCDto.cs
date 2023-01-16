using System.ComponentModel.DataAnnotations;
using Models.DTOs.Misc;

namespace Models.DTOs.General
{
    public class CPFCDto : IDto
    {
        [Required]
        [Range(0, 999.0)]
        public float Calories { get; set; }

        [Required]
        [Range(0, 999.0)]
        public float Proteins { get; set; }

        [Required]
        [Range(0, 999.0)]
        public float Fats { get; set; }

        [Required]
        [Range(0, 999.0)]
        public float Carbohydrates { get; set; }

        public CPFCDto()
        {
        }

        public CPFCDto(float calories, float proteins, float fats, float carbohydrates)
        {
            Calories = calories;
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }
    }
}