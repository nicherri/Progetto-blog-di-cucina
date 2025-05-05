using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimLocation", Schema = "Analytics")]
    public class DimLocation
    {
        [Key]
        public int LocationSK { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(10)]
        public string? CountryISOCode { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
