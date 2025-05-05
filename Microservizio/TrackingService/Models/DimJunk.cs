using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("DimJunk", Schema = "Analytics")]
    public class DimJunk
    {
        [Key]
        public int JunkSK { get; set; }

        public bool? IsFirstVisit { get; set; }
        public bool? IsLogged { get; set; }
        [MaxLength(20)]
        public string? UserAgentType { get; set; }
        public bool? IsSubscribed { get; set; }
        // altri flag/enumerazioni

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
