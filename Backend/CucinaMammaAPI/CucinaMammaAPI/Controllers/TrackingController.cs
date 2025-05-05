using CucinaMammaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackingController : ControllerBase
    {
        private readonly RabbitMqService _rabbitMqService;

        public TrackingController(RabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        /// <summary>
        /// Riceve un evento di tracciamento e lo invia a RabbitMQ.
        /// </summary>
        [HttpPost("log-event")]
        public async Task<IActionResult> LogEvent([FromBody] EventTracking eventTracking)
        {
            if (eventTracking == null)
            {
                return BadRequest("Dati evento mancanti nel body.");
            }
            if (string.IsNullOrEmpty(eventTracking.SessionId))
            {
                return BadRequest("SessionId è obbligatorio.");
            }
            if (string.IsNullOrEmpty(eventTracking.EventName))
            {
                return BadRequest("EventName è obbligatorio.");
            }

            // 🔥 MIGLIORIA: recupera IP server-side, se vuoi
            // var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Imposta l'ora UTC se non già impostata
            if (eventTracking.TimestampUtc == default)
            {
                eventTracking.TimestampUtc = DateTime.UtcNow;
            }

            try
            {
                await _rabbitMqService.PublishMessage(eventTracking);
            }
            catch (Exception ex)
            {
                // 🔥 MIGLIORIA: usa un logger qui
                return StatusCode(500, $"Errore invio a RabbitMQ: {ex.Message}");
            }

            return Ok(new { Message = "Evento inviato alla coda RabbitMQ con successo." });
        }
    }
}
 