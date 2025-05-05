using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimChannel", Schema = "Analytics")]
    public class DimChannel
    {
        [Key]
        public int ChannelSK { get; set; }

        [Required]
        [MaxLength(100)]
        public string? ChannelID { get; set; }

        [MaxLength(100)]
        public string? ChannelName { get; set; }

        [MaxLength(50)]
        public string? ChannelType { get; set; }

        public bool IsActive { get; set; }

        public DateTime DataInizioValidita { get; set; }
        public DateTime? DataFineValidita { get; set; }
        public bool IsCurrent { get; set; }
        public int Versione { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
