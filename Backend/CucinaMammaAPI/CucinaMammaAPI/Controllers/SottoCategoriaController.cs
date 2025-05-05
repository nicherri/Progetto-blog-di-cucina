using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CucinaMammaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SottoCategoriaController : ControllerBase
    {
        private readonly ISottoCategoriaRepository _repo;

        public SottoCategoriaController(ISottoCategoriaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SottoCategoriaDto>>> GetAll()
        {
            return Ok(await _repo.GetAllSottoCategorieAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SottoCategoriaDto>> GetById(int id)
        {
            var sc = await _repo.GetSottoCategoriaByIdAsync(id);
            if (sc == null)
                return NotFound(new { message = "Sottocategoria non trovata." });

            return Ok(sc);
        }

        [HttpPost]
        public async Task<ActionResult<SottoCategoria>> Create([FromBody] SottoCategoriaDto dto)
        {
            var model = new SottoCategoria
            {
                Nome = dto.Nome,
                Descrizione = dto.Descrizione
            };

            var created = await _repo.AddSottoCategoriaAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] SottoCategoriaDto dto, [FromForm] List<IFormFile>? nuoveImmagini, [FromForm] List<int>? immaginiDaRimuovere)
        {
            var success = await _repo.UpdateSottoCategoriaAsync(id, dto, nuoveImmagini, immaginiDaRimuovere);
            return success ? NoContent() : NotFound(new { message = "Sottocategoria non trovata." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteSottoCategoriaAsync(id);
            return deleted ? NoContent() : NotFound(new { message = "Sottocategoria non trovata." });
        }

        //Relazioni con Categoria e sottoCategoria



    }
}
