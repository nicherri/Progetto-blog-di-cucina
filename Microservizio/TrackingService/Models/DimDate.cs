using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimDate", Schema = "Analytics")]
    public class DimDate
    {
        [Key]
        public int DateSK { get; set; }

        [Required]
        public DateTime FullDate { get; set; }

        public int Year { get; set; }
        public int Quarter { get; set; }
        public int Month { get; set; }
        [MaxLength(20)]
        public string? MonthName { get; set; }
        public int Day { get; set; }
        public int DayOfWeek { get; set; }
        [MaxLength(20)]
        public string? DayNameOfWeek { get; set; }
        public int WeekOfYear { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsWeekend { get; set; }

        public int? FiscalYear { get; set; }
        public int? FiscalQuarter { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
