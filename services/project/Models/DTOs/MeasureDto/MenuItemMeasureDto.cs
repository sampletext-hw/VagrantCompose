using System.ComponentModel.DataAnnotations;
using Models.Db.Common;
using Models.DTOs.Misc;

namespace Models.DTOs.MeasureDto
{
    public class MenuItemMeasureDto : IDto
    {
        [Required]
        [EnumDataType(typeof(MeasureType))]
        public MeasureType MeasureType { get; set; }

        [Required]
        [Range(0, 9999.0)]
        public float Amount { get; set; }
    }
}