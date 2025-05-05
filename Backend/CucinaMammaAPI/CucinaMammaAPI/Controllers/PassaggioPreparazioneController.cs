using Microsoft.AspNetCore.Mvc;
using CucinaMammaAPI.Services;
using CucinaMammaAPI.DTOs;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CucinaMammaAPI.Controllers
{
    /// <summary>
    /// Controller per la gestione dei Passaggi di Preparazione.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PassaggioPreparazioneController : ControllerBase
    {
        private readonly IPassaggioPreparazioneService _passaggioService;

        /// <summary>
        /// Il costruttore riceve il PassaggioPreparazioneService tramite Dependency Injection.
        /// </summary>
        /// <param name="passaggioService"></param>
        public PassaggioPreparazioneController(IPassaggioPreparazioneService passaggioService)
        {
            _passaggioService = passaggioService;
        }

        // ============================================
        // GET: api/PassaggioPreparazione
        // ============================================
        /// <summary>
        /// Restituisce la lista completa di tutti i PassaggiPreparazione presenti nel sistema.
        /// </summary>
        /// <returns>Lista di PassaggioPreparazioneDTO.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassaggioPreparazioneDTO>>> GetAllPassaggi()
        {
            try
            {
                var passaggi = await _passaggioService.GetAllAsync();
                return Ok(passaggi);
            }
            catch (Exception ex)
            {
                // Se hai un middleware di gestione eccezioni globale, potresti semplicemente fare "throw".
                // Qui mostriamo un esempio di gestione locale
                return StatusCode(500, new { message = $"Errore interno: {ex.Message}" });
            }
        }

        // ===================================================
        // GET: api/PassaggioPreparazione/ricetta/{ricettaId}
        // ===================================================
        /// <summary>
        /// Restituisce la lista dei PassaggiPreparazione associati a una determinata ricetta (tramite ricettaId).
        /// </summary>
        /// <param name="ricettaId">ID della ricetta di cui si vogliono i passaggi</param>
        /// <returns>Lista di PassaggioPreparazioneDTO</returns>
        [HttpGet("ricetta/{ricettaId}")]
        public async Task<ActionResult<IEnumerable<PassaggioPreparazioneDTO>>> GetPassaggiByRicettaId(int ricettaId)
        {
            try
            {
                var passaggi = await _passaggioService.GetByRicettaIdAsync(ricettaId);
                return Ok(passaggi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ============================================
        // GET: api/PassaggioPreparazione/{id}
        // ============================================
        /// <summary>
        /// Restituisce un singolo PassaggioPreparazione in base al suo ID.
        /// </summary>
        /// <param name="id">ID del passaggio da cercare</param>
        /// <returns>Un PassaggioPreparazioneDTO o 404 se non trovato</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PassaggioPreparazioneDTO>> GetPassaggio(int id)
        {
            try
            {
                var passaggio = await _passaggioService.GetByIdAsync(id);
                if (passaggio == null)
                {
                    return NotFound(new { message = $"Il passaggio con ID={id} non esiste." });
                }

                return Ok(passaggio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ============================================
        // POST: api/PassaggioPreparazione
        // ============================================
        /// <summary>
        /// Crea un nuovo PassaggioPreparazione.
        /// </summary>
        /// <param name="dto">Il DTO contenente i dati del passaggio da creare.</param>
        /// <returns>Il passaggio appena creato, insieme al relativo codice 201 Created.</returns>
        [HttpPost]
        public async Task<ActionResult<PassaggioPreparazioneDTO>> CreatePassaggio([FromBody] PassaggioPreparazioneDTO dto)
        {
            try
            {
                // Esempio di validazione base con ModelState (valido se usi attributi di validazione su DTO/Model).
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Creiamo il nuovo passaggio
                var created = await _passaggioService.CreateAsync(dto);

                // Per generare un location header standard, puoi usare CreatedAtAction
                return CreatedAtAction(
                    nameof(GetPassaggio),
                    new { id = created.Id },
                    created
                );
            }
            catch (InvalidOperationException invEx)
            {
                // Esempio: gestione di eccezioni specifiche lanciate dal service
                return BadRequest(new { message = invEx.Message });
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(new { message = knfEx.Message });
            }
            catch (Exception ex)
            {
                // Catch generico per ogni altra eccezione
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ============================================
        // PUT: api/PassaggioPreparazione/{id}
        // ============================================
        /// <summary>
        /// Aggiorna i dati di un PassaggioPreparazione esistente.
        /// </summary>
        /// <param name="id">L'ID del passaggio da aggiornare</param>
        /// <param name="dto">I nuovi dati per l'aggiornamento</param>
        /// <returns>200 OK se l’update va a buon fine, 404 se non esiste, 400 se invalid.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePassaggio(int id, [FromBody] PassaggioPreparazioneDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _passaggioService.UpdateAsync(id, dto);
                if (!result)
                {
                    // Il passaggio non esisteva
                    return NotFound(new { message = $"Il passaggio con ID={id} non esiste." });
                }

                // Se l’update ha avuto successo, ritorno 200 (OK).
                // Oppure potresti restituire un NoContent() (204).
                return Ok(new { message = "Passaggio aggiornato con successo." });
            }
            catch (InvalidOperationException invEx)
            {
                // Esempio: ordine duplicato, conflitti di business, ecc.
                return BadRequest(new { message = invEx.Message });
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(new { message = knfEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // ============================================
        // DELETE: api/PassaggioPreparazione/{id}
        // ============================================
        /// <summary>
        /// Elimina un passaggio esistente. Se desiderato, il service può occuparsi di eliminare anche l'immagine associata.
        /// </summary>
        /// <param name="id">ID del passaggio da eliminare</param>
        /// <returns>200 OK (o 204 NoContent) se eliminato, 404 se non esiste</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePassaggio(int id)
        {
            try
            {
                var success = await _passaggioService.DeleteAsync(id);
                if (!success)
                {
                    return NotFound(new { message = $"Il passaggio con ID={id} non esiste o è già stato eliminato." });
                }

                // Ritorniamo 200 OK con un messaggio di conferma, oppure 204 NoContent.
                // A discrezione, ecco 200 con un messaggio:
                return Ok(new { message = $"Passaggio con ID={id} eliminato con successo." });

                // In alternativa:
                // return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
