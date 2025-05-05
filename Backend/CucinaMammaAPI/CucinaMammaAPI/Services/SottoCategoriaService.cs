using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CucinaMammaAPI.Services
{
    public class SottoCategoriaService : ISottoCategoriaRepository
    {
        private readonly AppDbContext _context;
        private readonly IImmagineService _immagineService;

        public SottoCategoriaService(AppDbContext context, IImmagineService immagineService)
        {
            _context = context;
            _immagineService = immagineService;
        }

        public async Task<List<SottoCategoriaDto>> GetAllSottoCategorieAsync()
        {
            var sottoCategorie = await _context.SottoCategorie
                .Include(sc => sc.Immagini)
                .ToListAsync();

            return sottoCategorie.Select(sc => new SottoCategoriaDto
            {
                Id = sc.Id,
                Nome = sc.Nome,
                Descrizione = sc.Descrizione,
                Immagini = sc.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            }).ToList();
        }

        public async Task<SottoCategoriaDto?> GetSottoCategoriaByIdAsync(int id)
        {
            var sc = await _context.SottoCategorie
                .Include(sc => sc.Immagini)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (sc == null) return null;

            return new SottoCategoriaDto
            {
                Id = sc.Id,
                Nome = sc.Nome,
                Descrizione = sc.Descrizione,
                Immagini = sc.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            };
        }

        public async Task<SottoCategoria> AddSottoCategoriaAsync(SottoCategoria sottoCategoria)
        {
            _context.SottoCategorie.Add(sottoCategoria);
            await _context.SaveChangesAsync();
            return sottoCategoria;
        }

        public async Task<bool> UpdateSottoCategoriaAsync(int id, SottoCategoriaDto dto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere)
        {
            var scEsistente = await _context.SottoCategorie
                .Include(sc => sc.Immagini)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (scEsistente == null)
                return false;

            scEsistente.Nome = dto.Nome;
            scEsistente.Descrizione = dto.Descrizione;

            if (immaginiDaRimuovere != null)
            {
                foreach (var imgId in immaginiDaRimuovere)
                {
                    await _immagineService.DeleteImmagineAsync(imgId);
                }
            }

            if (nuoveImmagini != null && nuoveImmagini.Any())
            {
                await _immagineService.UploadImmaginiAsync(EntitaTipo.SottoCategoria, id, nuoveImmagini);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSottoCategoriaAsync(int id)
        {
            var sc = await _context.SottoCategorie
                .Include(sc => sc.Immagini)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (sc == null) return false;

            foreach (var img in sc.Immagini)
            {
                await _immagineService.DeleteImmagineAsync(img.Id);
            }

            _context.SottoCategorie.Remove(sc);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SottoCategoriaExistsAsync(int id)
        {
            return await _context.SottoCategorie.AnyAsync(sc => sc.Id == id);
        }

        //Relazione categorie e sottocategorie
        public async Task<List<SottoCategoriaDto>> GetSottoCategorieByCategoriaIdAsync(int categoriaId)
        {
            var relazioni = await _context.CategorieSottoCategorie
                .Where(r => r.CategoriaId == categoriaId)
                .Include(r => r.SottoCategoria).ThenInclude(sc => sc.Immagini)
                .ToListAsync();

            return relazioni.Select(r => new SottoCategoriaDto
            {
                Id = r.SottoCategoria.Id,
                Nome = r.SottoCategoria.Nome,
                Descrizione = r.SottoCategoria.Descrizione,
                Immagini = r.SottoCategoria.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            }).ToList();
        }

        public async Task<bool> AddSottoCategoriaToCategoriaAsync(int categoriaId, int sottoCategoriaId)
        {
            var categoria = await _context.Categorie.FindAsync(categoriaId);
            var sottoCategoria = await _context.SottoCategorie.FindAsync(sottoCategoriaId);

            if (categoria == null || sottoCategoria == null)
                return false;

            var exists = await _context.CategorieSottoCategorie.AnyAsync(c =>
                c.CategoriaId == categoriaId && c.SottoCategoriaId == sottoCategoriaId);

            if (exists)
                return false;

            var relazione = new CategoriaSottoCategoria
            {
                CategoriaId = categoriaId,
                SottoCategoriaId = sottoCategoriaId
            };

            _context.CategorieSottoCategorie.Add(relazione);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveSottoCategoriaFromCategoriaAsync(int categoriaId, int sottoCategoriaId)
        {
            var relazione = await _context.CategorieSottoCategorie
                .FirstOrDefaultAsync(c => c.CategoriaId == categoriaId && c.SottoCategoriaId == sottoCategoriaId);

            if (relazione == null)
                return false;

            _context.CategorieSottoCategorie.Remove(relazione);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CategoriaSottoCategoriaExistsAsync(int categoriaId, int sottoCategoriaId)
        {
            return await _context.CategorieSottoCategorie
                .AnyAsync(c => c.CategoriaId == categoriaId && c.SottoCategoriaId == sottoCategoriaId);
        }

        public async Task<List<CategoriaDTO>> GetCategorieBySottoCategoriaIdAsync(int sottoCategoriaId)
        {
            var relazioni = await _context.CategorieSottoCategorie
                .Where(r => r.SottoCategoriaId == sottoCategoriaId)
                .Include(r => r.Categoria).ThenInclude(c => c.Immagini)
                .ToListAsync();

            return relazioni.Select(r => new CategoriaDTO
            {
                Id = r.Categoria.Id,
                Nome = r.Categoria.Nome,
                Descrizione = r.Categoria.Descrizione,
                Immagini = r.Categoria.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            }).ToList();
        }






    }
}
