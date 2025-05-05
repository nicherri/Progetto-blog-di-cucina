using CucinaMammaAPI.Data;
using CucinaMammaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/theme")]
[ApiController]
public class ThemeController : ControllerBase
{
    private readonly AppDbContext _context;

    public ThemeController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Recupera tutte le impostazioni del tema
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ThemeSetting>>> GetThemeSettings()
    {
        return await _context.ThemeSettings.ToListAsync();
    }

    // ✅ Aggiunge una nuova impostazione del tema
    [HttpPost]
    public async Task<IActionResult> CreateThemeSetting([FromBody] ThemeSetting themeSetting)
    {
        // Verifica se l'impostazione esiste già per evitare duplicati
        var existingSetting = await _context.ThemeSettings
            .FirstOrDefaultAsync(ts => ts.Component == themeSetting.Component && ts.Property == themeSetting.Property);

        if (existingSetting != null)
        {
            return BadRequest("L'impostazione esiste già. Usa PUT per aggiornarla.");
        }

        _context.ThemeSettings.Add(themeSetting);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetThemeSettings), new { id = themeSetting.Id }, themeSetting);
    }

    // ✅ Aggiorna o inserisce un'impostazione del tema
    [HttpPut]
    public async Task<IActionResult> UpdateThemeSetting([FromBody] ThemeSetting themeSetting)
    {
        var existingSetting = await _context.ThemeSettings
            .FirstOrDefaultAsync(ts => ts.Component == themeSetting.Component && ts.Property == themeSetting.Property);

        if (existingSetting == null)
        {
            _context.ThemeSettings.Add(themeSetting);
        }
        else
        {
            existingSetting.Value = themeSetting.Value;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
