using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TrackingService.Models
{
    [Table("FactSession", Schema = "Analytics")]
    public class FactSession
    {
        [Key]
        public long FactSessionSK { get; set; }

        public int SessionSK { get; set; }
        public int UserSK { get; set; }
        public int DateSK { get; set; }
        public int TimeSK { get; set; }
        public int? ChannelSK { get; set; }
        public int? DeviceSK { get; set; }
        public int? LocationSK { get; set; }
        public int? JunkSK { get; set; }

        public int? SessionDurationSeconds { get; set; }
        public int? PageViews { get; set; }
        public int? EventsCount { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
    }
}
