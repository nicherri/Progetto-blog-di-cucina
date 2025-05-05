using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Interfaces;
using CucinaMammaAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace CucinaMammaAPI.Services
{
    public class RicettaService : IRicettaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RicettaService> _logger;
        private readonly IIngredienteService _ingredienteService;
        private readonly IImmagineService _immagineService;
        private readonly IPassaggioPreparazioneService _passaggioService;
        private readonly ICategoriaRepository _categoriaService;

        public RicettaService(
            AppDbContext context,
            ILogger<RicettaService> logger,
            IIngredienteService ingredienteService,
            IImmagineService immagineService,
            IPassaggioPreparazioneService passaggioService,
            ICategoriaRepository categoriaService)
        {
            _context = context;
            _logger = logger;
            _ingredienteService = ingredienteService;
            _immagineService = immagineService;
            _passaggioService = passaggioService;
            _categoriaService = categoriaService;
        }

        // =============================================================================
        // GET ALL
        // =============================================================================
        public async Task<IEnumerable<RicettaDTO>> GetAllRicetteAsync()
        {
            var ricette = await _context.Ricette
                .Include(r => r.RicettaIngredienti).ThenInclude(ri => ri.Ingrediente)
                .Include(r => r.Immagini)
                .Include(r => r.PassaggiPreparazione).ThenInclude(p => p.Immagine)
                .Include(r => r.RicetteCategorie).ThenInclude(rc => rc.Categoria)
                .Include(r => r.Utente)
                .ToListAsync();

            return ricette.Select(r => MapToDTO(r)).ToList();
        }

        // =============================================================================
        // GET BY ID
        // =============================================================================
       public async Task<RicettaDTO?> GetRicettaByIdAsync(int id)
        {
            var ricetta = await _context.Ricette
                .Include(r => r.RicettaIngredienti).ThenInclude(ri => ri.Ingrediente)
                .Include(r => r.Immagini)
                .Include(r => r.PassaggiPreparazione).ThenInclude(p => p.Immagine)
                .Include(r => r.RicetteCategorie).ThenInclude(rc => rc.Categoria)
                .Include(r => r.Utente)
                .FirstOrDefaultAsync(r => r.Id == id);

            return ricetta == null ? null : MapToDTO(ricetta);
        }

        // =============================================================================
        // CREATE
        // =============================================================================
        public async Task<RicettaDTO> CreateRicettaAsync(RicettaDTO ricettaDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuovaRicetta = new Ricetta
                {
                    Titolo = ricettaDto.Titolo,
                    Descrizione = ricettaDto.Descrizione,
                    TempoPreparazione = ricettaDto.TempoPreparazione,
                    Difficolta = ricettaDto.Difficolta,
                    DataCreazione = DateTime.UtcNow,
                    UtenteId = ricettaDto.UtenteId
                };

                _context.Ricette.Add(nuovaRicetta);
                await _context.SaveChangesAsync();

                // Ingredienti
                if (ricettaDto.RicettaIngredienti != null)
                {
                    foreach (var riDto in ricettaDto.RicettaIngredienti)
                    {
                        _context.RicettaIngredienti.Add(new RicettaIngrediente
                        {
                            RicettaId = nuovaRicetta.Id,
                            IngredienteId = riDto.IngredienteId,
                            Quantita = riDto.Quantita,
                            UnitaMisura = riDto.UnitaMisura
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                // Passaggi
                if (ricettaDto.PassaggiPreparazione != null)
                {
                    foreach (var passaggioDto in ricettaDto.PassaggiPreparazione)
                    {
                        passaggioDto.RicettaId = nuovaRicetta.Id;
                        await _passaggioService.CreateAsync(passaggioDto);
                    }
                }

                // Categorie
                if (ricettaDto.Categorie != null)
                {
                    foreach (var catDto in ricettaDto.Categorie)
                    {
                        await _categoriaService.AddRicettaToCategoriaAsync(nuovaRicetta.Id, catDto.Id);
                    }
                }

                await transaction.CommitAsync();

                return await GetRicettaByIdAsync(nuovaRicetta.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la creazione di una nuova ricetta.");
                await transaction.RollbackAsync();
                throw;
            }
        }

        // =============================================================================
        // UPDATE
        // =============================================================================
        public async Task<RicettaDTO?> UpdateRicettaAsync(int id, RicettaDTO ricettaDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var ricettaEsistente = await _context.Ricette
                    .Include(r => r.Immagini)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (ricettaEsistente == null)
                {
                    _logger.LogWarning("Ricetta con ID {Id} non trovata per update.", id);
                    return null;
                }

                ricettaEsistente.Titolo = ricettaDto.Titolo;
                ricettaEsistente.Descrizione = ricettaDto.Descrizione;
                ricettaEsistente.Difficolta = ricettaDto.Difficolta;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetRicettaByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'aggiornamento della ricetta con ID {Id}", id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        // =============================================================================
        // DELETE
        // =============================================================================
        public async Task<bool> DeleteRicettaAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var ricettaEsistente = await _context.Ricette
                    .Include(r => r.Immagini)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (ricettaEsistente == null)
                {
                    return false;
                }

                _context.Ricette.Remove(ricettaEsistente);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante l'eliminazione della ricetta con ID {Id}", id);
                await transaction.RollbackAsync();
                throw;
            }
        }

        // =============================================================================
        // MAP TO DTO
        // =============================================================================
        private RicettaDTO MapToDTO(Ricetta ricetta)
        {
            return new RicettaDTO
            {
                Id = ricetta.Id,
                Titolo = ricetta.Titolo,
                Descrizione = ricetta.Descrizione,
                TempoPreparazione = ricetta.TempoPreparazione,
                Difficolta = ricetta.Difficolta,
                DataCreazione = ricetta.DataCreazione,
                UtenteId = ricetta.UtenteId,
                Immagini = ricetta.Immagini?.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList()
            };
        }
    }
}
