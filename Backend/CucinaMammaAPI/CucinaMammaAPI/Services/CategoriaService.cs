using Microsoft.EntityFrameworkCore;
using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using CucinaMammaAPI.Services;
using CucinaMammaAPI.Enums;
using System.Text.RegularExpressions;

namespace CucinaMammaAPI.Services
{
    /// <summary>
    /// Implementazione di ICategoriaRepository che fa sia da "service" che da "repository"
    /// in un'unica classe, gestendo CRUD, immagini e relazioni Ricetta-Categoria.
    /// </summary>
    public class CategoriaService : ICategoriaRepository
    {
        private readonly AppDbContext _context;
        private readonly IImmagineService _immagineService;

        /// <summary>
        /// Iniettiamo il DbContext per l’accesso al database e il service delle immagini.
        /// </summary>
        /// <param name="context">Contesto EF Core.</param>
        /// <param name="immagineService">Service per la gestione delle immagini.</param>
        public CategoriaService(AppDbContext context, IImmagineService immagineService)
        {
            _context = context;
            _immagineService = immagineService;
        }

        // =============================================================================
        // 1) CRUD DI BASE
        // =============================================================================

        /// <summary>
        /// Ottiene tutte le categorie dal database (in forma di Model).
        /// </summary>
        public async Task<List<CategoriaDTO>> GetAllCategorieAsync()
        {
            var categorie = await _context.Categorie
                .Include(c => c.Immagini) // ⬅️ Includi TUTTE le immagini
                .Include(c => c.RicetteCategorie).ThenInclude(rc => rc.Ricetta)
                .ToListAsync();

            return categorie.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Descrizione = c.Descrizione,
                Immagini = c.Immagini.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            }).ToList();
        }


        /// <summary>
        /// Ottiene una singola categoria (Model) tramite ID. Include l'immagine, se presente.
        /// </summary>
        public async Task<CategoriaDTO?> GetCategoriaByIdAsync(int id)
        {
            var categoria = await _context.Categorie
                .Include(c => c.Immagini) // ⬅️ Ora usiamo la lista di immagini
                .Include(c => c.RicetteCategorie).ThenInclude(rc => rc.Ricetta)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null) return null;

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome ?? string.Empty,
                Descrizione = categoria.Descrizione ?? string.Empty,

                // ✅ Adesso ritorniamo una lista di immagini
                Immagini = categoria.Immagini?.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url ?? string.Empty,
                    IsCover = img.IsCover
                }).ToList() ?? new List<ImmagineDTO>(),

                Ricette = categoria.RicetteCategorie?.Select(rc => new RicettaCategoriaDTO
                {
                    RicettaId = rc.Ricetta.Id,
                    Titolo = rc.Ricetta.Titolo ?? string.Empty
                }).ToList() ?? new List<RicettaCategoriaDTO>()
            };
        }


        /// <summary>
        /// Restituisce le ricette (in DTO) associate a una determinata categoria, 
        /// usando la tabella ponte RicettaCategoria.
        /// </summary>
        public async Task<List<RicettaDTO>> GetRicetteByCategoriaIdAsync(int categoriaId)
        {
            var ricette = await _context.RicetteCategorie
                .Where(rc => rc.CategoriaId == categoriaId)
                .Select(rc => new RicettaDTO
                {
                    Id = rc.Ricetta.Id,
                    Titolo = rc.Ricetta.Titolo,
                    Descrizione = rc.Ricetta.Descrizione
                })
                .ToListAsync();

            return ricette;
        }

        /// <summary>
        /// Aggiunge una nuova categoria (Model). 
        /// (Le immagini, se presenti, vanno caricate a parte con l'endpoint UploadImmaginiAsync)
        /// </summary>
        public async Task<Categoria> AddCategoriaAsync(Categoria categoria)
        {
            // 🧠 Generazione sicura dello slug, se non fornito
            categoria.Slug = string.IsNullOrWhiteSpace(categoria.Slug)
                ? GeneraSlug(categoria.Nome)
                : categoria.Slug.Trim().ToLowerInvariant();

            categoria.SeoTitle = categoria.SeoTitle?.Trim();
            categoria.SeoDescription = categoria.SeoDescription?.Trim();

            _context.Categorie.Add(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }


        /// <summary>
        /// Aggiorna una categoria esistente, partendo dall'ID e da un DTO che contiene i nuovi campi (nome, descrizione, ecc.)
        /// e anche l'eventuale immagine da aggiornare (solo metadati).
        /// Restituisce true se l'aggiornamento va a buon fine, altrimenti false.
        /// </summary>
        public async Task<bool> UpdateCategoriaAsync(int id, CategoriaDTO categoriaDto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere)
        {
            Console.WriteLine($"🔄 Avviato UpdateCategoriaAsync per ID: {id}");

            var categoriaEsistente = await _context.Categorie
                .Include(c => c.Immagini) // ⬅️ Includi tutte le immagini associate
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoriaEsistente == null)
            {
                Console.WriteLine($"❌ Categoria con ID {id} non trovata.");
                return false;
            }

            // 1️⃣ Aggiorna campi testuali
            Console.WriteLine($"📝 Aggiornamento Nome: {categoriaDto.Nome}, Descrizione: {categoriaDto.Descrizione}");
            categoriaEsistente.Nome = categoriaDto.Nome;
            categoriaEsistente.Descrizione = categoriaDto.Descrizione;

            categoriaEsistente.Slug = string.IsNullOrWhiteSpace(categoriaDto.Slug)
                ? GeneraSlug(categoriaDto.Nome)
                : categoriaDto.Slug.Trim().ToLowerInvariant();

            categoriaEsistente.SeoTitle = categoriaDto.SeoTitle?.Trim();
            categoriaEsistente.SeoDescription = categoriaDto.SeoDescription?.Trim();


            // 2️⃣ Rimuove immagini se richiesto
            if (immaginiDaRimuovere != null && immaginiDaRimuovere.Any())
            {
                Console.WriteLine($"🗑 Rimuovendo immagini: {string.Join(", ", immaginiDaRimuovere)}");
                foreach (var immagineId in immaginiDaRimuovere)
                {
                    var esitoRimozione = await _immagineService.DeleteImmagineAsync(immagineId);
                    Console.WriteLine(esitoRimozione ? $"✅ Immagine {immagineId} rimossa" : $"❌ Errore nel rimuovere l'immagine {immagineId}");
                }
            }
            else
            {
                Console.WriteLine("🔍 Nessuna immagine da rimuovere.");
            }

            // 3️⃣ Se ci sono nuove immagini, le aggiungiamo SENZA sostituire quelle esistenti
            if (nuoveImmagini != null && nuoveImmagini.Any())
            {
                Console.WriteLine($"📸 Nuove immagini ricevute: {nuoveImmagini.Count}");

                var immaginiCaricate = await _immagineService.UploadImmaginiAsync(
                    EntitaTipo.Categoria, categoriaEsistente.Id, nuoveImmagini
                );

                if (immaginiCaricate.Any())
                {
                    Console.WriteLine($"✅ Nuove immagini aggiunte: {string.Join(", ", immaginiCaricate.Select(i => i.Id))}");
                }
                else
                {
                    Console.WriteLine("❌ Nessuna immagine è stata salvata correttamente.");
                }
            }
            else
            {
                Console.WriteLine("🔍 Nessuna nuova immagine da aggiungere.");
            }

            // 4️⃣ Salviamo le modifiche
            await _context.SaveChangesAsync();
            Console.WriteLine($"💾 Categoria {id} aggiornata con successo!");

            return true;
        }

        // GeneraSlug(string input)

        private static string GeneraSlug(string input)
        {
            input = input.ToLowerInvariant().Trim();
            input = input
                .Replace("à", "a").Replace("è", "e").Replace("é", "e")
                .Replace("ì", "i").Replace("ò", "o").Replace("ù", "u");

            input = Regex.Replace(input, @"[^a-z0-9\s-]", "");    // rimuove caratteri speciali
            input = Regex.Replace(input, @"\s+", "-");            // sostituisce spazi con -
            input = Regex.Replace(input, @"-+", "-");             // rimuove ripetizioni di -

            return input.Trim('-');
        }


        /// <summary>
        /// Elimina una categoria esistente (se trovata). Se ha un'immagine associata, la elimina usando il service immagini.
        /// </summary>
        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorie
                .Include(c => c.Immagini) // ⬅️ Assicuriamoci di caricare tutte le immagini associate
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                return false;

            // 🔹 Ora gestiamo la rimozione di tutte le immagini associate
            if (categoria.Immagini != null && categoria.Immagini.Any())
            {
                Console.WriteLine($"🗑 Eliminando {categoria.Immagini.Count} immagini della categoria {id}...");
                foreach (var immagine in categoria.Immagini)
                {
                    await _immagineService.DeleteImmagineAsync(immagine.Id);
                }
            }
            else
            {
                Console.WriteLine($"🔍 Nessuna immagine trovata per la categoria {id}.");
            }

            // 🔹 Eliminiamo la categoria
            _context.Categorie.Remove(categoria);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Categoria {id} eliminata con successo.");
            return true;
        }


        /// <summary>
        /// Verifica se la categoria con un dato ID esiste nel DB.
        /// </summary>
        public async Task<bool> CategoriaExistsAsync(int id)
        {
            return await _context.Categorie.AnyAsync(c => c.Id == id);
        }

        // =============================================================================
        // 2) RELAZIONE RICETTA-CATEGORIA
        // =============================================================================

        /// <summary>
        /// Restituisce tutte le categorie (in DTO) a cui appartiene la ricetta specificata.
        /// </summary>
        public async Task<List<CategoriaDTO>> GetCategorieByRicettaIdAsync(int ricettaId)
        {
            var categorie = await _context.RicetteCategorie
                .Where(rc => rc.RicettaId == ricettaId)
                .Select(rc => new CategoriaDTO
                {
                    Id = rc.Categoria.Id,
                    Nome = rc.Categoria.Nome,
                    Descrizione = rc.Categoria.Descrizione
                })
                .ToListAsync();

            return categorie;
        }

        /// <summary>
        /// Aggiunge la relazione tra una ricetta esistente e una categoria esistente (molti-a-molti).
        /// </summary>
        public async Task<bool> AddRicettaToCategoriaAsync(int ricettaId, int categoriaId)
        {
            var esisteRelazione = await _context.RicetteCategorie
                .AnyAsync(rc => rc.RicettaId == ricettaId && rc.CategoriaId == categoriaId);

            if (esisteRelazione)
                return false; // Già associati

            var nuovaRelazione = new RicettaCategoria
            {
                RicettaId = ricettaId,
                CategoriaId = categoriaId
            };
            _context.RicetteCategorie.Add(nuovaRelazione);

            return (await _context.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Rimuove la relazione tra una ricetta e una categoria.
        /// </summary>
        public async Task<bool> RemoveRicettaFromCategoriaAsync(int ricettaId, int categoriaId)
        {
            var relazione = await _context.RicetteCategorie
                .FirstOrDefaultAsync(rc => rc.RicettaId == ricettaId && rc.CategoriaId == categoriaId);

            if (relazione == null)
                return false;

            _context.RicetteCategorie.Remove(relazione);
            return (await _context.SaveChangesAsync() > 0);
        }

        // 🔹 Ottiene tutte le SottoCategorie associate a una Categoria
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

        // 🔹 Ottiene tutte le Categorie associate a una SottoCategoria
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

        // 🔹 Aggiunge una SottoCategoria a una Categoria
        public async Task<bool> AddSottoCategoriaToCategoriaAsync(int categoriaId, int sottoCategoriaId)
        {
            var categoria = await _context.Categorie.FindAsync(categoriaId);
            var sottoCategoria = await _context.SottoCategorie.FindAsync(sottoCategoriaId);

            if (categoria == null || sottoCategoria == null)
                return false;

            var exists = await _context.CategorieSottoCategorie
                .AnyAsync(c => c.CategoriaId == categoriaId && c.SottoCategoriaId == sottoCategoriaId);

            if (exists)
                return false;

            _context.CategorieSottoCategorie.Add(new CategoriaSottoCategoria
            {
                CategoriaId = categoriaId,
                SottoCategoriaId = sottoCategoriaId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Rimuove una SottoCategoria da una Categoria
        public async Task<bool> RemoveSottoCategoriaFromCategoriaAsync(int categoriaId, int sottoCategoriaId)
        {
            var relazione = await _context.CategorieSottoCategorie
                .FirstOrDefaultAsync(r => r.CategoriaId == categoriaId && r.SottoCategoriaId == sottoCategoriaId);

            if (relazione == null)
                return false;

            _context.CategorieSottoCategorie.Remove(relazione);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔹 Verifica se esiste una relazione Categoria-SottoCategoria
        public async Task<bool> CategoriaSottoCategoriaExistsAsync(int categoriaId, int sottoCategoriaId)
        {
            return await _context.CategorieSottoCategorie
                .AnyAsync(r => r.CategoriaId == categoriaId && r.SottoCategoriaId == sottoCategoriaId);
        }

    }
}
