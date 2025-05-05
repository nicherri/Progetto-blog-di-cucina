using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CucinaMammaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CucinaMammaAPI.Services
{
    [ApiController]
    [Route("api/[controller]")]
    public class RicettaController : ControllerBase
    {
        private readonly IRicettaService _ricettaService;
        private readonly ILogger<RicettaController> _logger;

        public RicettaController(IRicettaService ricettaService, ILogger<RicettaController> logger)
        {
            _ricettaService = ricettaService;
            _logger = logger;
        }

        // =============================================================================
        // GET: api/ricetta
        // =============================================================================
        /// <summary>
        /// Recupera tutte le ricette.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RicettaDTO>>> GetAll()
        {
            try
            {
                var ricette = await _ricettaService.GetAllRicetteAsync();
                return Ok(ricette);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero di tutte le ricette.");
                return StatusCode(500, "Errore interno del server");
            }
        }
        // =============================================================================
        // GET: api/ricetta/{sitemap}
        // =============================================================================
        /// <summary>
        /// Restituisce solo le ricette pubblicate per la generazione della Sitemap.
        /// </summary>
        [HttpGet("sitemap")]
        public async Task<ActionResult<IEnumerable<object>>> GetRicetteForSitemap()
        {
            try
            {
                var ricette = await _ricettaService.GetAllRicetteAsync();

                var ricettePubblicate = ricette
                    .Where(r => r.Published) // Solo ricette pubblicate
                    .Select(r => new
                    {
                        url = $"https://tuosito.com/ricette/{r.Slug}", // URL SEO-Friendly
                        metaDescription = r.MetaDescription // ✅ Aggiunta la meta description
                    })
                    .ToList();

                return Ok(ricettePubblicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la generazione della Sitemap.");
                return StatusCode(500, "Errore interno del server");
            }
        }


        // =============================================================================
        // GET: api/ricetta/{id}
        // =============================================================================
        /// <summary>
        /// Recupera una singola ricetta per ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RicettaDTO>> GetById(int id)
        {
            try
            {
                var ricettaDto = await _ricettaService.GetRicettaByIdAsync(id);
                if (ricettaDto == null)
                {
                    return NotFound($"Nessuna ricetta trovata con ID={id}.");
                }
                return Ok(ricettaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante il recupero della ricetta con ID {Id}", id);
                return StatusCode(500, "Errore interno del server");
            }
        }

        // =============================================================================
        // POST: api/ricetta
        // =============================================================================
        /// <summary>
        /// Crea una nuova ricetta.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RicettaDTO>> Create([FromBody] RicettaDTO ricettaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuovaRicetta = await _ricettaService.CreateRicettaAsync(ricettaDto);
                // Restituiamo 201 Created con l'URL di dove reperire la nuova risorsa
                return CreatedAtAction(nameof(GetById), new { id = nuovaRicetta.Id }, nuovaRicetta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione di una nuova ricetta.");
                return StatusCode(500, "Errore interno del server");
            }
        }

        // =============================================================================
        // PUT: api/ricetta/{id}
        // =============================================================================
        /// <summary>
        /// Aggiorna una ricetta esistente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RicettaDTO>> Update(int id, [FromBody] RicettaDTO ricettaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var ricettaAggiornata = await _ricettaService.UpdateRicettaAsync(id, ricettaDto);
                if (ricettaAggiornata == null)
                {
                    return NotFound($"Nessuna ricetta trovata con ID={id}.");
                }

                return Ok(ricettaAggiornata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento della ricetta con ID {Id}", id);
                return StatusCode(500, "Errore interno del server");
            }
        }

        // =============================================================================
        // DELETE: api/ricetta/{id}
        // =============================================================================
        /// <summary>
        /// Elimina una ricetta esistente.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _ricettaService.DeleteRicettaAsync(id);
                if (!deleted)
                {
                    return NotFound($"Nessuna ricetta trovata con ID={id}.");
                }
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione della ricetta con ID {Id}", id);
                return StatusCode(500, "Errore interno del server");
            }
        }
    }
}
