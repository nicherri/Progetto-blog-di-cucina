using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Infrastructure.Errors;   // ← ProblemFactory
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using CucinaMammaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CucinaMammaAPI.Controllers;

/// <summary>API REST per la gestione delle categorie.</summary>
[ApiController]
[Route("api/[controller]")]
public sealed class CategoriaController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepo;
    private readonly IImmagineService _immagineService;
    private readonly AppDbContext _ctx;

    public CategoriaController(
        ICategoriaRepository categoriaRepo,
        IImmagineService immagineService,
        AppDbContext ctx)
    {
        _categoriaRepo = categoriaRepo;
        _immagineService = immagineService;
        _ctx = ctx;
    }

    /* ═════════════════════════════════════ 1) CRUD BASE ═════════════════════════════════════ */

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllCategorie()
    {
        var model = await _categoriaRepo.GetAllCategorieAsync();

        var dto = model.Select(cat => new CategoriaDTO
        {
            Id = cat.Id,
            Nome = cat.Nome,
            Descrizione = cat.Descrizione,
            Immagini = cat.Immagini.Select(i => new ImmagineDTO
            {
                Id = i.Id,
                Url = i.Url,
                IsCover = i.IsCover
            }).ToList()
        });

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoriaById(int id)
    {
        var cat = await _categoriaRepo.GetCategoriaByIdAsync(id);
        if (cat == null)
            return ProblemFactory.NotFound("Categoria", HttpContext).ToObjectResult();

        var dto = new CategoriaDTO
        {
            Id = cat.Id,
            Nome = cat.Nome,
            Descrizione = cat.Descrizione,
            Immagini = cat.Immagini.Select(i => new ImmagineDTO
            {
                Id = i.Id,
                Url = i.Url,
                IsCover = i.IsCover
            }).ToList()
        };
        return Ok(dto);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreaCategoria([FromForm] CategoriaDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            var errors = new Dictionary<string, string[]> { ["nome"] = ["Il nome è obbligatorio."] };
            return ProblemFactory.Validation(errors, HttpContext).ToObjectResult();
        }

        var nuova = new Categoria
        {
            Nome = dto.Nome.Trim(),
            Descrizione = dto.Descrizione?.Trim() ?? string.Empty,
            Slug = dto.Slug?.Trim(),
            SeoTitle = dto.SeoTitle?.Trim(),
            SeoDescription = dto.SeoDescription?.Trim()
        };

        var creata = await _categoriaRepo.AddCategoriaAsync(nuova);

        // upload singola immagine se presente
        if (dto.ImmagineFile is not null)
        {
            var imgs = await _immagineService.UploadImmaginiAsync(
                            EntitaTipo.Categoria, creata.Id,
                            new List<IFormFile> { dto.ImmagineFile });

            var img = imgs.FirstOrDefault();
            if (img is not null)
            {
                creata.Immagini.Add(new Immagine
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover,
                    CategoriaId = creata.Id
                });
                await _ctx.SaveChangesAsync();
            }
        }

        return CreatedAtAction(nameof(GetCategoriaById),
                               new { id = creata.Id },
                               new
                               {
                                   creata.Id,
                                   creata.Nome,
                                   creata.Slug,
                                   creata.SeoTitle,
                                   creata.SeoDescription
                               });
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateCategoria(
        int id,
        [FromForm] CategoriaDTO dto,
        [FromForm] List<IFormFile>? nuoveImmagini,
        [FromForm] List<int>? immaginiDaRimuovere)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            var errors = new Dictionary<string, string[]> { ["nome"] = ["Il nome è obbligatorio."] };
            return ProblemFactory.Validation(errors, HttpContext).ToObjectResult();
        }

        if (!await _categoriaRepo.CategoriaExistsAsync(id))
            return ProblemFactory.NotFound("Categoria", HttpContext).ToObjectResult();

        var ok = await _categoriaRepo.UpdateCategoriaAsync(id, dto, nuoveImmagini, immaginiDaRimuovere);
        if (!ok)
            return ProblemFactory.Internal("Aggiornamento categoria fallito", HttpContext).ToObjectResult();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoriaAsync(int id)
    {
        var cat = await _ctx.Categorie
                            .Include(c => c.Immagini)
                            .FirstOrDefaultAsync(c => c.Id == id);

        if (cat is null)
            return ProblemFactory.NotFound("Categoria", HttpContext).ToObjectResult();

        // rimuovi immagini collegate
        foreach (var img in cat.Immagini.ToList())
            await _immagineService.DeleteImmagineAsync(img.Id);

        _ctx.Categorie.Remove(cat);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    /* ═════════════════╗ RELAZIONI RICETTE ╔═════════════════ */

    [HttpGet("{id}/ricette")]
    public async Task<IActionResult> GetRicetteByCategoriaId(int id)
    {
        if (!await _categoriaRepo.CategoriaExistsAsync(id))
            return ProblemFactory.NotFound("Categoria", HttpContext).ToObjectResult();

        var ricette = await _categoriaRepo.GetRicetteByCategoriaIdAsync(id);
        return Ok(ricette);
    }

    [HttpGet("ricette/{ricettaId}")]
    public async Task<IActionResult> GetCategorieByRicettaId(int ricettaId)
    {
        var categorie = await _categoriaRepo.GetCategorieByRicettaIdAsync(ricettaId);
        return Ok(categorie);
    }

    [HttpPost("{id}/ricette/{ricettaId}")]
    public async Task<IActionResult> AddRicettaToCategoria(int id, int ricettaId)
    {
        var ok = await _categoriaRepo.AddRicettaToCategoriaAsync(ricettaId, id);
        if (!ok)
            return ProblemFactory.Conflict("Ricetta già associata alla categoria", HttpContext)
                                 .ToObjectResult();

        return NoContent();
    }

    [HttpDelete("{id}/ricette/{ricettaId}")]
    public async Task<IActionResult> RemoveRicettaFromCategoria(int id, int ricettaId)
    {
        var ok = await _categoriaRepo.RemoveRicettaFromCategoriaAsync(ricettaId, id);
        if (!ok)
            return ProblemFactory.NotFound("Relazione ricetta-categoria", HttpContext).ToObjectResult();

        return NoContent();
    }

    /* ═════════════════╗ RELAZIONI SOTTOCATEGORIE ╔═════════════════ */

    [HttpGet("{id}/sottocategorie")]
    public async Task<IActionResult> GetSottoCategorieByCategoriaId(int id)
    {
        if (!await _categoriaRepo.CategoriaExistsAsync(id))
            return ProblemFactory.NotFound("Categoria", HttpContext).ToObjectResult();

        var sotto = await _categoriaRepo.GetSottoCategorieByCategoriaIdAsync(id);
        return Ok(sotto);
    }

    [HttpGet("sottocategorie/{sottoCategoriaId}/categorie")]
    public async Task<IActionResult> GetCategorieBySottoCategoriaId(int sottoCategoriaId)
    {
        var cat = await _categoriaRepo.GetCategorieBySottoCategoriaIdAsync(sottoCategoriaId);
        return Ok(cat);
    }
}
