using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimTime", Schema = "Analytics")]
    public class DimTime
    {
        [Key]
        public int TimeSK { get; set; }

        public byte Hour { get; set; }   // 0..23
        public byte Minute { get; set; } // 0..59
        [MaxLength(2)]
        public string? AM_PM { get; set; }
        [MaxLength(20)]
        public string? TimeSlot { get; set; }
        [MaxLength(5)]
        public string? QuarterHourSlot { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
