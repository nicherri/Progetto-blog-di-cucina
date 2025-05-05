using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimFunnelStep", Schema = "Analytics")]
    public class DimFunnelStep
    {
        [Key]
        public int FunnelStepSK { get; set; }

        [MaxLength(100)]
        public string? FunnelStepName { get; set; }
        public int FunnelStageOrder { get; set; }

        [MaxLength(100)]
        public string? FunnelName { get; set; }
        public bool IsCurrent { get; set; } = true;
        public int Version { get; set; } = 1;
        public DateTime? DataInizioValidita { get; set; }
        public DateTime? DataFineValidita { get; set; }
        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
