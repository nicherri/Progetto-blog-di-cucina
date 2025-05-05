using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrackingService.Data;
using SharedModels.Models; 
namespace TrackingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TrackingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Riceve un evento di tracking e lo salva nel database.
        /// </summary>
        [HttpPost("log-event")]
        public async Task<IActionResult> LogEvent([FromBody] EventTracking eventTracking)
        {
            if (eventTracking == null || string.IsNullOrEmpty(eventTracking.SessionId))
                return BadRequest("Dati evento non validi. SessionId è obbligatorio.");

            // Controlla se la sessione esiste nel database
            var session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == eventTracking.SessionId);

            if (session == null)
            {
                // Creiamo la sessione perché non esiste
                session = new SessionTracking
                {
                    Id = eventTracking.SessionId,
                    UserId = eventTracking.UserId,
                    StartTimeUtc = DateTime.UtcNow,
                    IpAddress = "0.0.0.0", // Cambia con un valore reale se disponibile
                    BrowserInfo = "Unknown",
                    DeviceType = "Unknown",
                    OperatingSystem = "Unknown",
                    Locale = "it-IT",
                    PageViews = 1, // Evita valori NULL
                    OptOut = false
                };

                _dbContext.Sessions.Add(session);
                await _dbContext.SaveChangesAsync(); // 🔥 Salvataggio immediato della sessione
            }

            // **🔍 Verifica nuovamente se la sessione è stata salvata**
            session = await _dbContext.Sessions.FirstOrDefaultAsync(s => s.Id == eventTracking.SessionId);
            if (session == null)
            {
                return StatusCode(500, $"Errore: la sessione con ID '{eventTracking.SessionId}' non è stata salvata correttamente.");
            }

            // **🔥 Ora possiamo salvare l'evento in sicurezza**
            eventTracking.TimestampUtc = DateTime.UtcNow;
            _dbContext.Events.Add(eventTracking);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Errore nel salvataggio dell'evento: {ex.InnerException?.Message}");
            }

            return Ok(new { Message = "Evento salvato correttamente" });
        }


        /// <summary>
        /// Ottiene tutti gli eventi di tracking.
        /// </summary>
        [HttpGet("all-events")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _dbContext.Events.ToListAsync();
            return Ok(events);
        }

        /// <summary>
        /// Ottiene gli eventi di tracking per una sessione specifica.
        /// </summary>
        [HttpGet("events-by-session/{sessionId}")]
        public async Task<IActionResult> GetEventsBySession(string sessionId)
        {
            var events = await _dbContext.Events
                .Where(e => e.SessionId == sessionId) // ✅ Conversione corretta
                .ToListAsync();

            if (!events.Any())
                return NotFound(new { Message = "Nessun evento trovato per questa sessione" });

            return Ok(events);
        }

        /// <summary>
        /// Ottiene le informazioni di una sessione specifica.
        /// </summary>
        [HttpGet("session/{id}")]
        public async Task<IActionResult> GetSessionById(string id)
        {
            var session = await _dbContext.Sessions
                .FirstOrDefaultAsync(s => s.Id == id); // ✅ Corretto: Usare Id invece di SessionId

            if (session == null)
                return NotFound(new { Message = "Sessione non trovata" });

            return Ok(session);
        }
    }
}
