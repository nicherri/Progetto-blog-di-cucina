using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackingService.Data
{
    [Table("DimSession", Schema = "Analytics")]
    public class DimSession
    {
        [Key]
        public int SessionSK { get; set; }

        [Required]
        [MaxLength(100)]
        public string? SessionID { get; set; } // nvarchar(100) NOT NULL

        // datetime2(7) NULL
        public DateTime? SessionStart { get; set; }

        // datetime2(7) NULL
        public DateTime? SessionEnd { get; set; }

        [MaxLength(50)]
        public string? SessionStatus { get; set; }  // se la colonna può essere NULL in DB

        [MaxLength(50)]
        public string? TipoSessione { get; set; }   // "TipoSessione" colonna

        // DataInizioValidita (datetime2(7) NOT NULL)
        public DateTime DataInizioValidita { get; set; }

        // DataFineValidita (datetime2(7) NULL)
        public DateTime? DataFineValidita { get; set; }

        // IsCurrent (bit NOT NULL)
        public bool IsCurrent { get; set; }

        // Versione (int NOT NULL)
        public int Versione { get; set; }

        // DataCaricamento (datetime2(7) NULL)
        public DateTime? DataCaricamento { get; set; }

        [MaxLength(512)]
        public string? BrowserInfo { get; set; }

        [MaxLength(45)]
        public string? IpAddress { get; set; }

        public bool? OptOut { get; set; }

        public int? PageViews { get; set; }

        [MaxLength(10)]
        public string? Locale { get; set; }

        [MaxLength(100)]
        public string? OperatingSystem { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? UserId { get; set; }

        // Ecco la colonna DeviceType (nvarchar(50)), se presente in DB
        [MaxLength(50)]
        public string? DeviceType { get; set; }
    }
}
