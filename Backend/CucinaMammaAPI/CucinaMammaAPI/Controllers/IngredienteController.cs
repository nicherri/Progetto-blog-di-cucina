using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredienteController : ControllerBase
    {
        private readonly IIngredienteService _ingredienteService;
        private readonly ILogger<IngredienteController> _logger;

        public IngredienteController(IIngredienteService ingredienteService, ILogger<IngredienteController> logger)
        {
            _ingredienteService = ingredienteService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera tutti gli ingredienti disponibili.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredienteDTO>>> GetAll()
        {
            try
            {
                var ingredienti = await _ingredienteService.GetAllIngredientiAsync();
                return Ok(ingredienti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in GetAll()");
                return StatusCode(500, "Errore durante il recupero degli ingredienti.");
            }
        }

        /// <summary>
        /// Recupera un ingrediente specifico per ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<IngredienteDTO>> GetById(int id)
        {
            try
            {
                var ingrediente = await _ingredienteService.GetIngredienteByIdAsync(id);
                if (ingrediente == null)
                    return NotFound($"Ingrediente con ID {id} non trovato.");

                return Ok(ingrediente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in GetById({Id})", id);
                return StatusCode(500, "Errore durante il recupero dell'ingrediente.");
            }
        }

        /// <summary>
        /// Crea un nuovo ingrediente.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<IngredienteDTO>> Create([FromForm] IngredienteDTO ingredienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ingredienteCreato = await _ingredienteService.CreateIngredienteAsync(ingredienteDto);
                return CreatedAtAction(nameof(GetById), new { id = ingredienteCreato.Id }, ingredienteCreato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in Create()");
                return StatusCode(500, "Errore durante la creazione dell'ingrediente.");
            }
        }


        /// <summary>
        /// Aggiorna un ingrediente esistente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<IngredienteDTO>> Update(int id, [FromForm] IngredienteDTO ingredienteDto, [FromForm] List<IFormFile>? nuoveImmagini, [FromForm] List<int>? immaginiDaRimuovere)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ingredienteAggiornato = await _ingredienteService.UpdateIngredienteAsync(id, ingredienteDto, nuoveImmagini, immaginiDaRimuovere);
                if (ingredienteAggiornato == null)
                {
                    return NotFound($"Ingrediente con ID {id} non trovato.");
                }
                return Ok(ingredienteAggiornato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in Update({Id})", id);
                return StatusCode(500, "Si è verificato un errore durante l'aggiornamento dell'ingrediente.");
            }
        }



        /// <summary>
        /// Elimina un ingrediente esistente.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _ingredienteService.DeleteIngredienteAsync(id);
                if (!success)
                    return NotFound($"Ingrediente con ID {id} non trovato.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in Delete({Id})", id);
                return StatusCode(500, "Errore durante l'eliminazione dell'ingrediente.");
            }
        }

        // ==============================
        // GESTIONE RELAZIONE RICETTA-INGREDIENTE
        // ==============================

        /// <summary>
        /// Recupera tutti gli ingredienti associati a una ricetta.
        /// </summary>
        [HttpGet("ricetta/{ricettaId}/ingredienti")]
        public async Task<ActionResult<IEnumerable<RicettaIngredienteDTO>>> GetIngredientiByRicetta(int ricettaId)
        {
            try
            {
                var ingredienti = await _ingredienteService.GetIngredientiByRicettaIdAsync(ricettaId);
                return Ok(ingredienti);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in GetIngredientiByRicetta({RicettaId})", ricettaId);
                return StatusCode(500, "Errore durante il recupero degli ingredienti della ricetta.");
            }
        }

        /// <summary>
        /// Aggiunge un ingrediente a una ricetta.
        /// </summary>
        [HttpPost("ricetta/{ricettaId}/aggiungi")]
        public async Task<ActionResult<RicettaIngredienteDTO>> AddIngredienteToRicetta([FromBody] RicettaIngredienteDTO dto)
        {
            try
            {
                var result = await _ingredienteService.AddIngredienteToRicettaAsync(dto);
                return CreatedAtAction(nameof(GetIngredientiByRicetta), new { ricettaId = result.RicettaId }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in AddIngredienteToRicetta()");
                return StatusCode(500, "Errore durante l'aggiunta dell'ingrediente alla ricetta.");
            }
        }

        /// <summary>
        /// Rimuove un ingrediente da una ricetta.
        /// </summary>
        [HttpDelete("ricetta/{ricettaId}/ingrediente/{ingredienteId}")]
        public async Task<IActionResult> RemoveIngredienteFromRicetta(int ricettaId, int ingredienteId)
        {
            try
            {
                var success = await _ingredienteService.RemoveIngredienteFromRicettaAsync(ricettaId, ingredienteId);
                if (!success)
                    return NotFound($"Ingrediente con ID {ingredienteId} non associato alla ricetta {ricettaId}.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in RemoveIngredienteFromRicetta({RicettaId}, {IngredienteId})", ricettaId, ingredienteId);
                return StatusCode(500, "Errore durante la rimozione dell'ingrediente dalla ricetta.");
            }
        }

        /// <summary>
        /// Aggiorna la quantità o l'unità di misura di un ingrediente in una ricetta.
        /// </summary>
        [HttpPut("ricetta/{ricettaId}/ingrediente/{ingredienteId}")]
        public async Task<ActionResult<RicettaIngredienteDTO>> UpdateRicettaIngrediente(int ricettaId, int ingredienteId, [FromBody] RicettaIngredienteDTO dto)
        {
            try
            {
                var result = await _ingredienteService.UpdateRicettaIngredienteAsync(ricettaId, ingredienteId, dto);
                if (result == null)
                    return NotFound($"Ingrediente con ID {ingredienteId} non trovato nella ricetta {ricettaId}.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in UpdateRicettaIngrediente({RicettaId}, {IngredienteId})", ricettaId, ingredienteId);
                return StatusCode(500, "Errore durante l'aggiornamento dell'ingrediente nella ricetta.");
            }
        }
    }
}
