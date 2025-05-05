using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models
{
    public class SessionTracking
    {
        [Key]
        [Required]
        [MaxLength(100)]
        public string Id { get; set; }
        /// <summary>
        /// Identificativo utente, se disponibile (es. email, ID di Identity, o altro).
        /// Sarà null se l’utente non è autenticato.
        /// </summary>
        [MaxLength(100)]
        public string? UserId { get; set; }

        /// <summary>
        /// Indirizzo IP, eventualmente anonimizzato per GDPR. 
        /// Consigliato limitare la lunghezza a 45 per IPv6.
        /// </summary>
        [Required]
        [MaxLength(45)]
        public string IpAddress { get; set; }

        /// <summary>
        /// Informazioni dettagliate sul browser (user agent),
        /// incluse versione e motore di rendering.
        /// </summary>
        [MaxLength(512)]
        public string BrowserInfo { get; set; }

        /// <summary>
        /// Informazioni sul tipo di dispositivo (desktop, tablet, mobile, smartTV, etc.).
        /// </summary>
        [MaxLength(50)]
        public string DeviceType { get; set; }

        /// <summary>
        /// Sistema operativo rilevato (es. Windows, iOS, Android).
        /// </summary>
        [MaxLength(100)]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Lingua/paese dell’utente (es. "it-IT").
        /// </summary>
        [MaxLength(10)]
        public string Locale { get; set; }

        /// <summary>
        /// Pagina di ingresso: utile per capire da dove l’utente
        /// è atterrato sul sito di cucina.
        /// </summary>
        [MaxLength(500)]
        public string? EntryUrl { get; set; }

        /// <summary>
        /// Eventuale URL di uscita, se tracciato in tempo reale o post-elaborazione.
        /// </summary>
        [MaxLength(500)]
        public string? ExitUrl { get; set; }

        /// <summary>
        /// Parametri di marketing UTM (source, medium, campaign, content, term).
        /// Questi campi consentono un’analisi approfondita delle campagne.
        /// </summary>
        [MaxLength(200)]
        public string? UtmSource { get; set; }

        [MaxLength(200)]
        public string? UtmMedium { get; set; }

        [MaxLength(200)]
        public string? UtmCampaign { get; set; }

        [MaxLength(200)]
        public string? UtmContent { get; set; }

        [MaxLength(200)]
        public string? UtmTerm { get; set; }

        /// <summary>
        /// Timestamp di inizio sessione. Si raccomanda di usare UTC per coerenza.
        /// </summary>
        public DateTime StartTimeUtc { get; set; }

        /// <summary>
        /// Timestamp di fine sessione, se noto.
        /// </summary>
        public DateTime? EndTimeUtc { get; set; }

        /// <summary>
        /// Numero di pagine viste durante la sessione (o calcolato in post-processing).
        /// </summary>
        public int PageViews { get; set; }

        /// <summary>
        /// Preferenze riguardanti la privacy (opt-out),
        /// true se l’utente non desidera alcun tracciamento.
        /// </summary>
        public bool OptOut { get; set; }

        /// <summary>
        /// Informazioni geografiche più dettagliate: se usi un servizio di geolocalizzazione
        /// puoi salvare il Paese, la Regione/Provincia e la Città stimata.
        /// </summary>
        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? Region { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        /// <summary>
        /// Eventuale fuso orario rilevato dal browser (o preferito dall’utente).
        /// </summary>
        [MaxLength(50)]
        public string? TimeZone { get; set; }

        public virtual ICollection<EventTracking> Events { get; set; } = new List<EventTracking>();


        /// <summary>
        /// Proprietà calcolata che ritorna la durata della sessione (EndTimeUtc - StartTimeUtc),
        /// se EndTimeUtc è valorizzato.
        /// </summary>
        [NotMapped]
        public TimeSpan? Duration
        {
            get
            {
                if (EndTimeUtc.HasValue)
                {
                    return EndTimeUtc.Value - StartTimeUtc;
                }
                return null;
            }
        }
    }
}