using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackingService.Data
{
    [Table("FactEvent", Schema = "Analytics")]
    public class FactEvent
    {
        [Key]
        public long FactEventSK { get; set; }

        // Colonne INT? (se in DB possono essere NULL)
        public int? UserSK { get; set; }
        public int? EventSK { get; set; }
        public int? DateSK { get; set; }
        public int? TimeSK { get; set; }
        public int? SessionSK { get; set; }
        public int? ChannelSK { get; set; }
        public int? DeviceSK { get; set; }
        public int? LocationSK { get; set; }
        public int? JunkSK { get; set; }

        // decimal(18,2)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? EventValue { get; set; }

        public int? ScrollDepthPercentage { get; set; }

        // decimal(10,2)
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TimeSpentSeconds { get; set; }

        // CustomLabel (nvarchar(200))
        [MaxLength(200)]
        public string? CustomLabel { get; set; }

        public int? TrackingVersion { get; set; }

        // DataCaricamento (datetime2(7))
        public DateTime? DataCaricamento { get; set; }

        // MouseX, MouseY, ScrollX, ScrollY, ...
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }
        public int? ScrollX { get; set; }
        public int? ScrollY { get; set; }
        public int? ViewportWidth { get; set; }
        public int? ViewportHeight { get; set; }
        public int? ElementLeft { get; set; }
        public int? ElementTop { get; set; }
        public int? ElementWidth { get; set; }
        public int? ElementHeight { get; set; }

        // FunnelStep (nvarchar(100))
        [MaxLength(100)]
        public string? FunnelStep { get; set; }

        // FunnelData (nvarchar(200))
        [MaxLength(200)]
        public string? FunnelData { get; set; }

        // ReplayChunkData (nvarchar(MAX))
        [Column(TypeName = "nvarchar(max)")]
        public string? ReplayChunkData { get; set; }

        // ReplayChunkType (nvarchar(50))
        [MaxLength(50)]
        public string? ReplayChunkType { get; set; }

        public int? FunnelStepSK { get; set; }


        [MaxLength(100)]
        public string? EventName { get; set; }

        [MaxLength(100)]
        public string? EventCategory { get; set; }

        [MaxLength(100)]
        public string? EventLabel { get; set; }

        public DateTime? TimestampUtc { get; set; }

    }
}
