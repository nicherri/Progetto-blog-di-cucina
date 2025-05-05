using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace CucinaMammaAPI.Services
{
    public class IngredienteService : IIngredienteService
    {
        private readonly AppDbContext _context;
        private readonly IImmagineService _immagineService;
        private readonly ILogger<IngredienteService> _logger;

        public IngredienteService(AppDbContext context, IImmagineService immagineService, ILogger<IngredienteService> logger)
        {
            _context = context;
            _immagineService = immagineService;
            _logger = logger;
        }

        // 📌 Recupera tutti gli ingredienti con immagini
        public async Task<IEnumerable<IngredienteDTO>> GetAllIngredientiAsync()
        {
            var ingredienti = await _context.Ingredienti
                .Include(i => i.RicettaIngredienti)
                    .ThenInclude(ri => ri.Ricetta)
                .Include(i => i.Immagini)
                .ToListAsync();

            return ingredienti.Select(MapIngredienteToDTO);
        }

        // 📌 Recupera un singolo ingrediente per ID con immagini
        public async Task<IngredienteDTO?> GetIngredienteByIdAsync(int id)
        {
            var ingrediente = await _context.Ingredienti
                .Include(i => i.RicettaIngredienti)
                    .ThenInclude(ri => ri.Ricetta)
                .Include(i => i.Immagini)
                .FirstOrDefaultAsync(i => i.Id == id);

            return ingrediente == null ? null : MapIngredienteToDTO(ingrediente);
        }

        // 📌 Crea un nuovo ingrediente con immagini (se presenti)
        public async Task<IngredienteDTO> CreateIngredienteAsync(IngredienteDTO ingredienteDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var nuovoIngrediente = new Ingrediente
                {
                    Nome = ingredienteDto.Nome
                };

                _context.Ingredienti.Add(nuovoIngrediente);
                await _context.SaveChangesAsync(); // 🔵 IMPORTANTE: Salviamo l'ingrediente PRIMA delle immagini

                // 📸 Salviamo le immagini e le associamo all'ingrediente
                List<ImmagineDTO> immaginiSalvate = new();
                if (ingredienteDto.FileImmagini != null && ingredienteDto.FileImmagini.Any())
                {
                    Console.WriteLine($"📸 Caricamento di {ingredienteDto.FileImmagini.Count} immagini per l'ingrediente {nuovoIngrediente.Id}");

                    immaginiSalvate = await _immagineService.UploadImmaginiAsync(EntitaTipo.Ingrediente, nuovoIngrediente.Id, ingredienteDto.FileImmagini);

                    // 🔴 IMPORTANTE: Rimuoviamo il tracking prima di aggiornare
                    _context.ChangeTracker.Clear();

                    // 🔵 Creiamo una nuova lista di immagini con la corretta associazione
                    var immaginiAssociate = immaginiSalvate.Select(imgDTO => new Immagine
                    {
                        Id = imgDTO.Id,
                        Url = imgDTO.Url,
                        IsCover = imgDTO.IsCover,
                        IngredienteId = nuovoIngrediente.Id // 🟢 ASSEGNA CORRETTAMENTE L'ID DELL'INGREDIENTE
                    }).ToList();

                    // ✅ Aggiungiamo esplicitamente le immagini al database
                    foreach (var immagine in immaginiAssociate)
                    {
                        _context.Immagini.Update(immagine);
                    }

                    // ✅ Aggiungiamo le immagini all'ingrediente
                    nuovoIngrediente.Immagini = immaginiAssociate;
                    _context.Ingredienti.Update(nuovoIngrediente); // 🔵 ASSICURIAMOCI CHE L'ENTITÀ VENGA SALVATA
                }

                await _context.SaveChangesAsync(); // 🔴 FINALIZZIAMO LE MODIFICHE AL DATABASE
                await transaction.CommitAsync();

                Console.WriteLine($"✅ Ingrediente {nuovoIngrediente.Id} creato con {immaginiSalvate.Count} immagini.");

                return new IngredienteDTO
                {
                    Id = nuovoIngrediente.Id,
                    Nome = nuovoIngrediente.Nome,
                    Immagini = immaginiSalvate
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Errore durante la creazione dell'ingrediente");
                throw;
            }
        }


        // 📌 Aggiorna un ingrediente e gestisce immagini
        public async Task<IngredienteDTO?> UpdateIngredienteAsync(int id, IngredienteDTO ingredienteDto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere)
        {
            Console.WriteLine($"🔄 Avviato UpdateIngredienteAsync per ID: {id}");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Recupera l'ingrediente esistente con le immagini associate
                var ingredienteEsistente = await _context.Ingredienti
                    .Include(i => i.Immagini)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (ingredienteEsistente == null)
                {
                    Console.WriteLine($"❌ Ingrediente con ID {id} non trovato.");
                    return null;
                }

                // 2️⃣ Aggiorna i campi testuali
                Console.WriteLine($"📝 Aggiornamento Nome: {ingredienteDto.Nome}");
                ingredienteEsistente.Nome = ingredienteDto.Nome;

                // 3️⃣ Rimuove immagini se richiesto
                if (immaginiDaRimuovere != null && immaginiDaRimuovere.Any())
                {
                    Console.WriteLine($"🗑 Rimuovendo immagini: {string.Join(", ", immaginiDaRimuovere)}");

                    foreach (var immagineId in immaginiDaRimuovere)
                    {
                        var immagineEsistente = await _context.Immagini
                            .FirstOrDefaultAsync(img => img.Id == immagineId);

                        if (immagineEsistente != null)
                        {
                            _context.Immagini.Remove(immagineEsistente); // ✅ Rimuoviamo dal contesto
                            await _immagineService.DeleteImmagineAsync(immagineId);
                            Console.WriteLine($"✅ Immagine {immagineId} rimossa con successo!");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Immagine {immagineId} non trovata nel database.");
                        }
                    }

                    // ✅ Aggiorniamo la lista delle immagini rimanenti
                    ingredienteEsistente.Immagini = ingredienteEsistente.Immagini
                        .Where(img => !immaginiDaRimuovere.Contains(img.Id))
                        .ToList();
                }
                else
                {
                    Console.WriteLine("🔍 Nessuna immagine da rimuovere.");
                }

                // 4️⃣ Se ci sono nuove immagini, le carichiamo
                if (nuoveImmagini != null && nuoveImmagini.Any())
                {
                    Console.WriteLine($"📸 Nuove immagini ricevute: {nuoveImmagini.Count}");

                    var immaginiCaricateDTO = await _immagineService.UploadImmaginiAsync(EntitaTipo.Ingrediente, ingredienteEsistente.Id, nuoveImmagini);

                    if (immaginiCaricateDTO.Any())
                    {
                        Console.WriteLine($"✅ Nuove immagini aggiunte: {string.Join(", ", immaginiCaricateDTO.Select(i => i.Id))}");

                        // 🔴 IMPORTANTE: Rimuoviamo il tracking prima di aggiungere nuove immagini
                        _context.ChangeTracker.Clear();

                        // 🔵 Creiamo una nuova lista di immagini per evitare il conflitto di tracking
                        var immaginiCaricate = immaginiCaricateDTO.Select(imgDTO => new Immagine
                        {
                            Id = imgDTO.Id,
                            Url = imgDTO.Url,
                            IsCover = imgDTO.IsCover,
                            IngredienteId = ingredienteEsistente.Id // 🟢 ASSEGNA CORRETTAMENTE L'ID DELL'INGREDIENTE
                        }).ToList();

                        // ✅ Aggiungiamo solo immagini non duplicate
                        foreach (var immagine in immaginiCaricate)
                        {
                            if (!ingredienteEsistente.Immagini.Any(img => img.Id == immagine.Id))
                            {
                                ingredienteEsistente.Immagini.Add(immagine);
                                _context.Immagini.Update(immagine); // 🔴 AGGIORNA ESPRESSAMENTE IL CAMPO NEL DB
                            }
                        }
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

                // 5️⃣ Salviamo le modifiche
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"💾 Ingrediente {id} aggiornato con successo!");

                // 6️⃣ Ricarichiamo l'ingrediente per restituire le immagini aggiornate
                var ingredienteAggiornato = await _context.Ingredienti
                    .Include(i => i.Immagini)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == id);

                return new IngredienteDTO
                {
                    Id = ingredienteAggiornato.Id,
                    Nome = ingredienteAggiornato.Nome,
                    Immagini = ingredienteAggiornato.Immagini.Select(img => new ImmagineDTO
                    {
                        Id = img.Id,
                        Url = img.Url,
                        IsCover = img.IsCover
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Errore durante l'aggiornamento dell'ingrediente con ID {Id}", id);
                throw;
            }
        }


        //Elimina ingrediente
        public async Task<bool> DeleteIngredienteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var ingrediente = await _context.Ingredienti
                    .Include(i => i.Immagini)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (ingrediente == null)
                {
                    _logger.LogWarning("Ingrediente con ID {Id} non trovato per l'eliminazione.", id);
                    return false;
                }

                // 🟢 Creiamo una copia della lista per evitare modifiche mentre la iteriamo
                var immaginiDaEliminare = ingrediente.Immagini.ToList();

                // 🟢 Eliminiamo le immagini collegate all'ingrediente
                foreach (var immagine in immaginiDaEliminare)
                {
                    await _immagineService.DeleteImmagineAsync(immagine.Id);
                }

                // 🟢 Rimuoviamo l'ingrediente solo dopo aver rimosso le immagini
                _context.Ingredienti.Remove(ingrediente);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Ingrediente con ID {Id} eliminato con successo.", id);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Errore durante l'eliminazione dell'ingrediente con ID {Id}", id);
                throw;
            }
        }


        // 📌 Aggiunge un ingrediente a una ricetta
        public async Task<RicettaIngredienteDTO> AddIngredienteToRicettaAsync(RicettaIngredienteDTO ricettaIngredienteDto)
        {
            var entity = new RicettaIngrediente
            {
                RicettaId = ricettaIngredienteDto.RicettaId,
                IngredienteId = ricettaIngredienteDto.IngredienteId,
                Quantita = ricettaIngredienteDto.Quantita,
                UnitaMisura = ricettaIngredienteDto.UnitaMisura
            };

            _context.RicettaIngredienti.Add(entity);
            await _context.SaveChangesAsync();

            return ricettaIngredienteDto;
        }

        // 📌 Recupera tutti gli ingredienti di una ricetta
        public async Task<IEnumerable<RicettaIngredienteDTO>> GetIngredientiByRicettaIdAsync(int ricettaId)
        {
            var ingredienti = await _context.RicettaIngredienti
                .Where(ri => ri.RicettaId == ricettaId)
                .Include(ri => ri.Ingrediente)
                .ToListAsync();

            return ingredienti.Select(ri => new RicettaIngredienteDTO
            {
                RicettaId = ri.RicettaId,
                IngredienteId = ri.IngredienteId,
                Quantita = ri.Quantita,
                UnitaMisura = ri.UnitaMisura
            });
        }

        // 📌 Rimuove un ingrediente da una ricetta
        public async Task<bool> RemoveIngredienteFromRicettaAsync(int ricettaId, int ingredienteId)
        {
            var entity = await _context.RicettaIngredienti
                .FirstOrDefaultAsync(ri => ri.RicettaId == ricettaId && ri.IngredienteId == ingredienteId);

            if (entity == null)
                return false;

            _context.RicettaIngredienti.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        // 📌 Aggiorna un ingrediente in una ricetta

        public async Task<RicettaIngredienteDTO?> UpdateRicettaIngredienteAsync(int ricettaId, int ingredienteId, RicettaIngredienteDTO ricettaIngredienteDto)
        {
            var entity = await _context.RicettaIngredienti
                .FirstOrDefaultAsync(ri => ri.RicettaId == ricettaId && ri.IngredienteId == ingredienteId);

            if (entity == null)
                return null;

            entity.Quantita = ricettaIngredienteDto.Quantita;
            entity.UnitaMisura = ricettaIngredienteDto.UnitaMisura;

            await _context.SaveChangesAsync();
            return ricettaIngredienteDto;
        }

        // 📌 Metodo di mappatura da `Ingrediente` a `IngredienteDTO`
        private IngredienteDTO MapIngredienteToDTO(Ingrediente ingrediente)
        {
            return new IngredienteDTO
            {
                Id = ingrediente.Id,
                Nome = ingrediente.Nome,
                RicettaIngredienti = ingrediente.RicettaIngredienti?.Select(ri => new RicettaIngredienteDTO
                {
                    RicettaId = ri.RicettaId,
                    IngredienteId = ri.IngredienteId,
                    Quantita = ri.Quantita,
                    UnitaMisura = ri.UnitaMisura
                }).ToList() ?? new List<RicettaIngredienteDTO>(),
                Immagini = ingrediente.Immagini?.Select(img => new ImmagineDTO
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsCover = img.IsCover
                }).ToList() ?? new List<ImmagineDTO>()
            };
        }
    }
}
