using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using CucinaMammaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CucinaMammaAPI.Controllers
{
    /// <summary>
    /// Controller per la gestione delle Categorie, che si appoggia a ICategoriaRepository
    /// (implementato da CategoriaService).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IImmagineService _immagineService;

        private readonly AppDbContext _context;

        /// <summary>
        /// Iniettiamo l'interfaccia del repository, che in realtà è istanziata come CategoriaService.
        /// </summary>
        /// <param name="categoriaRepo">Implementazione di ICategoriaRepository (CategoriaService).</param>
        public CategoriaController(
     ICategoriaRepository categoriaRepo,
     IImmagineService immagineService, 
     AppDbContext context) 
        {
            _categoriaRepo = categoriaRepo;
            _immagineService = immagineService;  
            _context = context;  
        }

        // =============================================================================
        // 1) CRUD BASE
        // =============================================================================

        /// <summary>
        /// Restituisce tutte le categorie presenti nel DB.
        /// Nota: Il metodo del repository ritorna List<Categoria> (Model),
        /// quindi qui facciamo la mappatura in DTO prima di rispondere al client.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllCategorie()
        {
            var categorieModel = await _categoriaRepo.GetAllCategorieAsync();

            var categorieDto = categorieModel.Select(cat => new CategoriaDTO
            {
                Id = cat.Id,
                Nome = cat.Nome,
                Descrizione = cat.Descrizione,
                Immagini = cat.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            }).ToList();

            return Ok(categorieDto);
        }


        /// <summary>
        /// Recupera una singola categoria per ID, se esiste.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoriaById(int id)
        {
            var categoria = await _categoriaRepo.GetCategoriaByIdAsync(id);
            if (categoria == null)
                return NotFound(new { message = $"Categoria con ID {id} non trovata." });

            var categoriaDto = new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Descrizione = categoria.Descrizione,
                Immagini = categoria.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            };

            return Ok(categoriaDto);
        }


        /// <summary>
        /// Crea una nuova categoria. 
        /// Nota: l'interfaccia richiede di usare AddCategoriaAsync(Categoria),
        /// quindi convertiamo da DTO a Model prima di chiamarlo.
        /// </summary>


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreaCategoria([FromForm] CategoriaDTO categoriaDto)
        {
            if (string.IsNullOrWhiteSpace(categoriaDto.Nome))
            {
                return BadRequest(new { message = "Il nome della categoria è obbligatorio" });
            }

            // 🔁 Mappatura completa da DTO a Model (lo slug verrà gestito nel Service)
            var nuovaCategoria = new Categoria
            {
                Nome = categoriaDto.Nome.Trim(),
                Descrizione = categoriaDto.Descrizione?.Trim() ?? string.Empty,
                Slug = categoriaDto.Slug?.Trim(), // anche se nullo va bene, il Service lo genera
                SeoTitle = categoriaDto.SeoTitle?.Trim(),
                SeoDescription = categoriaDto.SeoDescription?.Trim()
            };

            // 🔹 Salva categoria senza immagini per ottenere l'ID
            var categoriaCreata = await _categoriaRepo.AddCategoriaAsync(nuovaCategoria);

            // 📸 Se è presente un'immagine iniziale (singola), la carichiamo
            if (categoriaDto.ImmagineFile != null)
            {
                var immaginiCaricate = await _immagineService.UploadImmaginiAsync(
                    EntitaTipo.Categoria,
                    categoriaCreata.Id,
                    new List<IFormFile> { categoriaDto.ImmagineFile }
                );

                if (immaginiCaricate.Any())
                {
                    var immagine = immaginiCaricate.First();
                    categoriaCreata.Immagini.Add(new Immagine
                    {
                        Id = immagine.Id,
                        Url = immagine.Url,
                        IsCover = immagine.IsCover,
                        CategoriaId = categoriaCreata.Id
                    });

                    // 🔄 Salva associazione immagine
                    await _context.SaveChangesAsync();
                }
            }

            // ✅ Torniamo la categoria creata (puoi mappare su un DTO se preferisci)
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoriaCreata.Id }, new
            {
                categoriaCreata.Id,
                categoriaCreata.Nome,
                categoriaCreata.Slug,
                categoriaCreata.SeoTitle,
                categoriaCreata.SeoDescription
            });
        }

        /// <summary>
        /// Aggiorna i campi di una categoria esistente (nome, descrizione e/o immagine).
        /// L'interfaccia ha un metodo UpdateCategoriaAsync(int, CategoriaDTO), 
        /// quindi passiamo ID e DTO direttamente.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCategoria(
     int id,
     [FromForm] CategoriaDTO categoriaDto,
     [FromForm] List<IFormFile>? nuoveImmagini,
     [FromForm] List<int>? immaginiDaRimuovere)
        {
            if (string.IsNullOrWhiteSpace(categoriaDto.Nome))
                return BadRequest(new { message = "Il campo 'Nome' è obbligatorio." });

            var categoriaEsistente = await _categoriaRepo.GetCategoriaByIdAsync(id);
            if (categoriaEsistente == null)
                return NotFound(new { message = $"Categoria con ID {id} non trovata." });

            // 🔄 Eseguiamo l'aggiornamento, compresi slug e meta SEO
            var esito = await _categoriaRepo.UpdateCategoriaAsync(id, categoriaDto, nuoveImmagini, immaginiDaRimuovere);

            if (!esito)
                return StatusCode(500, new { message = "Errore nell'aggiornamento della categoria." });

            return NoContent();
        }


        /// <summary>
        /// Elimina una categoria esistente (ID).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorie
                .Include(c => c.Immagini) // Includi le immagini per rimuoverle
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return NotFound(new { message = $"Categoria con ID {id} non trovata." });

            // 🔹 Rimuoviamo le immagini prima di eliminare la categoria
            foreach (var img in categoria.Immagini.ToList()) // Converti in lista per evitare problemi di enumerazione
            {
                await _immagineService.DeleteImmagineAsync(img.Id);
            }

            _context.Categorie.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // =============================================================================
        // 2) RELAZIONI RICETTA-CATEGORIA
        // =============================================================================

        /// <summary>
        /// Recupera tutte le ricette (DTO) associate a una certa categoria.
        /// </summary>
        [HttpGet("{id}/ricette")]
        public async Task<IActionResult> GetRicetteByCategoriaId(int id)
        {
            // 1. Controlliamo se la categoria esiste 
            if (!await _categoriaRepo.CategoriaExistsAsync(id))
                return NotFound(new { message = $"Categoria con ID {id} non trovata." });

            // 2. Recuperiamo le ricette in DTO
            var ricette = await _categoriaRepo.GetRicetteByCategoriaIdAsync(id);
            return Ok(ricette);
        }

        /// <summary>
        /// Recupera tutte le categorie (DTO) a cui appartiene una data ricetta.
        /// </summary>
        [HttpGet("ricette/{ricettaId}")]
        public async Task<IActionResult> GetCategorieByRicettaId(int ricettaId)
        {
            var categorie = await _categoriaRepo.GetCategorieByRicettaIdAsync(ricettaId);
            return Ok(categorie);
        }

        /// <summary>
        /// Aggiunge la relazione molti-a-molti tra una categoria (id) e una ricetta (ricettaId).
        /// </summary>
        [HttpPost("{id}/ricette/{ricettaId}")]
        public async Task<IActionResult> AddRicettaToCategoria(int id, int ricettaId)
        {
            var esito = await _categoriaRepo.AddRicettaToCategoriaAsync(ricettaId, id);
            if (!esito)
                return BadRequest(new { message = $"Impossibile aggiungere ricetta {ricettaId} alla categoria {id} (già associati o dati non validi)." });

            return NoContent();
        }

        /// <summary>
        /// Rimuove la relazione molti-a-molti tra una categoria (id) e una ricetta (ricettaId).
        /// </summary>
        [HttpDelete("{id}/ricette/{ricettaId}")]
        public async Task<IActionResult> RemoveRicettaFromCategoria(int id, int ricettaId)
        {
            var esito = await _categoriaRepo.RemoveRicettaFromCategoriaAsync(ricettaId, id);
            if (!esito)
                return BadRequest(new { message = $"Impossibile rimuovere ricetta {ricettaId} dalla categoria {id} (relazione inesistente?)." });

            return NoContent();
        }

        // =============================================================================
        [HttpGet("{id}/sottocategorie")]
        public async Task<IActionResult> GetSottoCategorieByCategoriaId(int id)
        {
            if (!await _categoriaRepo.CategoriaExistsAsync(id))
                return NotFound(new { message = $"Categoria con ID {id} non trovata." });

            var sottoCategorie = await _categoriaRepo.GetSottoCategorieByCategoriaIdAsync(id);
            return Ok(sottoCategorie);
        }

        [HttpGet("sottocategorie/{sottoCategoriaId}/categorie")]
        public async Task<IActionResult> GetCategorieBySottoCategoriaId(int sottoCategoriaId)
        {
            var categorie = await _categoriaRepo.GetCategorieBySottoCategoriaIdAsync(sottoCategoriaId);
            return Ok(categorie);
        }


    }
}
