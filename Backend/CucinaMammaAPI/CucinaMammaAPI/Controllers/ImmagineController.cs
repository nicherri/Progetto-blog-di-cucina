using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;




namespace CucinaMammaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImmagineController : ControllerBase
    {
        private readonly IImmagineService _immagineService;

        public ImmagineController(IImmagineService immagineService)
        {
            _immagineService = immagineService;
        }

        /// <summary>
        /// Restituisce tutte le immagini presenti nel sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImmagineDTO>>> GetAllImmagini()
        {
            var immagini = await _immagineService.GetAllImmaginiAsync();
            return Ok(immagini);
        }

        /// <summary>
        /// Restituisce una singola immagine in base al suo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ImmagineDTO>> GetImmagineById(int id)
        {
            var immagine = await _immagineService.GetImmagineByIdAsync(id);
            if (immagine == null)
                return NotFound(new { message = "Immagine non trovata." });

            return Ok(immagine);
        }

        /// <summary>
        /// Carica una o più immagini per un'entità specificata tramite 'tipo' e 'entitaId'.
        /// Le immagini vanno inviate come file in form-data (key='files').
        /// Esempio: POST /api/immagine/upload?tipo=Categoria&entitaId=3
        [HttpPost("upload")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<List<ImmagineDTO>>> UploadImmagini(
    [FromQuery] EntitaTipo tipo,
    [FromQuery] int entitaId,
    [FromForm] List<IFormFile> files,
    [FromForm] IFormFile? metadati) // 👈 cambiato tipo da string a IFormFile
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { message = "Nessun file caricato." });
            }

            List<ImmagineDTO>? listaMetadati = null;

            if (metadati != null)
            {
                using var reader = new StreamReader(metadati.OpenReadStream());
                var json = await reader.ReadToEndAsync();

                try
                {

                    listaMetadati = JsonSerializer.Deserialize<List<ImmagineDTO>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // 👈 ESSENZIALE
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Errore parsing metadati", dettaglio = ex.Message });
                }
            }

            // DEBUG
            if (listaMetadati != null)
            {
                for (int i = 0; i < listaMetadati.Count; i++)
                {
                    var m = listaMetadati[i];
                    Console.WriteLine($"📋 Metadata per file {i}:\n  Alt: {m.Alt}\n  Title: {m.Title}\n  Caption: {m.Caption}\n  SEO: {m.NomeFileSeo}\n  Ordine: {m.Ordine}\n  Cover: {m.IsCover}");
                }
            }

            var result = await _immagineService.UploadImmaginiAsync(tipo, entitaId, files, listaMetadati);
            return Ok(result);
        }


        /// <summary>
        /// Aggiorna metadati di un'immagine (ad esempio IsCover).
        /// Non carica un nuovo file, ma permette di modificare campi come IsCover.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImmagine(int id, [FromBody] ImmagineDTO immagineDto)
        {
            // Esempio di piccola validazione
            if (immagineDto == null)
                return BadRequest(new { message = "I dati dell'immagine sono obbligatori." });

            var updatedImmagine = await _immagineService.UpdateImmagineAsync(id, immagineDto);

            if (updatedImmagine == null)
                return NotFound(new { message = "Immagine non trovata." });

            // In genere si restituisce 200 o 204
            return NoContent();
        }

        /// <summary>
        /// Elimina un'immagine (file fisico + record DB).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImmagine(int id)
        {
            var deleted = await _immagineService.DeleteImmagineAsync(id);
            if (!deleted)
                return NotFound(new { message = "Immagine non trovata o già eliminata." });

            return NoContent();
        }
    }
}
