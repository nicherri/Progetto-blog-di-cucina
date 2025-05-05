using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{

    [Table("DimDevice", Schema = "Analytics")]
    public class DimDevice
    {
        [Key]
        public int DeviceSK { get; set; }

        [Required]
        [MaxLength(100)]
        public string? DeviceID { get; set; }

        [MaxLength(50)]
        public string? DeviceType { get; set; }

        [MaxLength(50)]
        public string? OS { get; set; }

        [MaxLength(50)]
        public string? Browser { get; set; }

        [MaxLength(20)]
        public string? BrowserVersion { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
