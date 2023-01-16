using System;
using System.ComponentModel.DataAnnotations;
using Models.Attributes;
using Models.DTOs.Misc;

namespace Models.DTOs.General
{
    public class OpenCloseTimeDto : IDto
    {
        [Required]
        [DataType(DataType.Time)]
        [Range(typeof(TimeSpan), "00:00", "23:59")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan Open { get; set; }
        
        [Required]
        [DataType(DataType.Time)]
        [Range(typeof(TimeSpan), "00:00", "23:59")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        [LargerThan(nameof(Open))]
        public TimeSpan Close { get; set; }
        
        [Range(0, 6)]
        public uint DayOfWeek { get; set; }
        
        [Required]
        public bool IsWorking { get; set; }
    }
}