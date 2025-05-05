using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimUser", Schema = "Analytics")]
    public class DimUser
    {
        [Key]
        public int UserSK { get; set; }

        [Required]
        [MaxLength(100)]
        public string? UserID { get; set; }

        [MaxLength(100)]
        public string? Nome { get; set; }
        [MaxLength(255)]
        public string? Email { get; set; }
        [MaxLength(50)]
        public string? Telefono { get; set; }
        [MaxLength(50)]
        public string? Ruolo { get; set; }
        [MaxLength(50)]
        public string? SegmentoMarketing { get; set; }

        public DateTime? DataRegistrazione { get; set; }

        public DateTime DataInizioValidita { get; set; }
        public DateTime? DataFineValidita { get; set; }
        public bool IsCurrent { get; set; }
        public int Versione { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
