using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimEvent", Schema = "Analytics")]
    public class DimEvent
    {
        [Key]
        public int EventSK { get; set; }

        [Required]
        [MaxLength(100)]
        public string? EventID { get; set; }

        [MaxLength(100)]
        public string? EventName { get; set; }

        [MaxLength(100)]
        public string? EventCategory { get; set; }

        [MaxLength(100)]
        public string? EventLabel { get; set; }

        public DateTime DataInizioValidita { get; set; }
        public DateTime? DataFineValidita { get; set; }
        public bool IsCurrent { get; set; }
        public int Versione { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
