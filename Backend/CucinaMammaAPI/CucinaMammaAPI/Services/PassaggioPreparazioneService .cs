using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CucinaMammaAPI.Services
{
    public class PassaggioPreparazioneService : IPassaggioPreparazioneService
    {
        private readonly AppDbContext _context;
        private readonly IImmagineService _immagineService;

        public PassaggioPreparazioneService(
            AppDbContext context,
            IImmagineService immagineService)
        {
            _context = context;
            _immagineService = immagineService;
        }

        /// <summary>
        /// Restituisce la lista di tutti i passaggi di preparazione (inclusa l'immagine se presente).
        /// </summary>
        public async Task<List<PassaggioPreparazioneDTO>> GetAllAsync()
        {
            var passaggi = await _context.PassaggiPreparazione
                .Include(p => p.Immagine)
                .ToListAsync();

            return passaggi.Select(p => MapToDTO(p)).ToList();
        }

        /// <summary>
        /// Restituisce i passaggi di preparazione appartenenti a una ricetta, in ordine.
        /// </summary>
        public async Task<List<PassaggioPreparazioneDTO>> GetByRicettaIdAsync(int ricettaId)
        {
            var passaggi = await _context.PassaggiPreparazione
                .Include(p => p.Immagine)
                .Where(p => p.RicettaId == ricettaId)
                .OrderBy(p => p.Ordine)
                .ToListAsync();

            return passaggi.Select(p => MapToDTO(p)).ToList();
        }

        /// <summary>
        /// Restituisce un singolo passaggio, se esiste.
        /// </summary>
        public async Task<PassaggioPreparazioneDTO?> GetByIdAsync(int id)
        {
            var passaggio = await _context.PassaggiPreparazione
                .Include(p => p.Immagine)
                .FirstOrDefaultAsync(p => p.Id == id);

            return passaggio == null ? null : MapToDTO(passaggio);
        }

        /// <summary>
        /// Crea un nuovo passaggio (senza caricare file).
        /// Se desideri un'immagine, dovrai caricarla in un passaggio successivo (upload file).
        /// </summary>
        public async Task<PassaggioPreparazioneDTO> CreateAsync(PassaggioPreparazioneDTO passaggioDto)
        {
            // Esempio: controlliamo che non ci sia già un passaggio con lo stesso Ordine
            var existingSameOrder = await _context.PassaggiPreparazione
                .AnyAsync(p =>
                    p.RicettaId == passaggioDto.RicettaId &&
                    p.Ordine == passaggioDto.Ordine);

            if (existingSameOrder)
            {
                throw new InvalidOperationException(
                    $"Esiste già un passaggio con Ordine={passaggioDto.Ordine} per la Ricetta ID={passaggioDto.RicettaId}."
                );
            }

            // Creiamo l'entità di base
            var newPassaggio = new PassaggioPreparazione
            {
                Ordine = passaggioDto.Ordine,
                Descrizione = passaggioDto.Descrizione,
                RicettaId = passaggioDto.RicettaId
            };

            // NOTA: se passaggioDto.Immagine != null e passaggioDto.Immagine.Id == 0,
            // potresti volere "Aggiungere" un'immagine, ma ora non esiste AggiungiImmagineAsync.
            // Per coerenza, facciamo un "2 passaggi" (crei passaggio, poi /api/immagine/upload?tipo=Passaggio&entitaId=...)

            _context.PassaggiPreparazione.Add(newPassaggio);
            await _context.SaveChangesAsync();

            // Ricarichiamo
            var createdEntity = await _context.PassaggiPreparazione
                .Include(p => p.Immagine)
                .FirstOrDefaultAsync(p => p.Id == newPassaggio.Id);

            return MapToDTO(createdEntity);
        }

        /// <summary>
        /// Aggiorna un passaggio esistente, senza caricamento fisico di file (usare upload multiplo a parte).
        /// </summary>
        public async Task<bool> UpdateAsync(int id, PassaggioPreparazioneDTO passaggioDto)
        {
            var passaggioToUpdate = await _context.PassaggiPreparazione
                .FirstOrDefaultAsync(p => p.Id == id);

            if (passaggioToUpdate == null)
                return false;

            // Se l'ordine o la ricetta sono cambiati, controlla collisioni
            if (passaggioToUpdate.Ordine != passaggioDto.Ordine ||
                passaggioToUpdate.RicettaId != passaggioDto.RicettaId)
            {
                var conflict = await _context.PassaggiPreparazione
                    .AnyAsync(p =>
                        p.RicettaId == passaggioDto.RicettaId &&
                        p.Ordine == passaggioDto.Ordine &&
                        p.Id != id
                    );

                if (conflict)
                {
                    throw new InvalidOperationException(
                        $"Non è possibile assegnare Ordine={passaggioDto.Ordine} perché è già usato in un altro passaggio " +
                        $"per la Ricetta ID={passaggioDto.RicettaId}."
                    );
                }
            }

            // Aggiorna i campi testuali
            passaggioToUpdate.Ordine = passaggioDto.Ordine;
            passaggioToUpdate.Descrizione = passaggioDto.Descrizione;
            passaggioToUpdate.RicettaId = passaggioDto.RicettaId;

            // Gestione immagine 
            // se passaggioDto.Immagine == null => possiamo disassociare l'immagine (ImmagineId = null)
            // o lasciare invariato, a seconda di come preferisci gestire.

            // Esempio: disassocia se passaggioDto.Immagine==null
            if (passaggioDto.Immagine == null)
            {
                passaggioToUpdate.ImmagineId = null;
            }
            else
            {
                // Se ID=0 => utente vuole aggiungere immagine, ma qui non c'è AggiungiImmagineAsync
                // => deve usare upload file.
                // Se ID>0 => vuole associare un'immagine esistente, 
                //           o aggiornare i metadati con UpdateImmagineAsync.

                if (passaggioDto.Immagine.Id > 0)
                {
                    // Aggiorna i metadati 
                    var updatedImg = await _immagineService.UpdateImmagineAsync(
                        passaggioDto.Immagine.Id,
                        passaggioDto.Immagine
                    );

                    if (updatedImg == null)
                    {
                        throw new KeyNotFoundException(
                            $"Immagine con ID={passaggioDto.Immagine.Id} non trovata."
                        );
                    }

                    // Associa l'immagine aggiornata
                    passaggioToUpdate.ImmagineId = updatedImg.Id;
                }
                else
                {
                    // ID=0 => metodo rimosso
                    // => flusso "due passaggi": devi fare un upload via /api/immagine/upload?tipo=Passaggio...
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Elimina un passaggio esistente dal DB (senza o con eventuale eliminazione dell'immagine se serve).
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var passaggioToDelete = await _context.PassaggiPreparazione
                .FirstOrDefaultAsync(p => p.Id == id);

            if (passaggioToDelete == null)
                return false;

            // Se vuoi, elimina l'immagine:
            // if (passaggioToDelete.ImmagineId.HasValue)
            // {
            //     await _immagineService.DeleteImmagineAsync(passaggioToDelete.ImmagineId.Value);
            // }

            _context.PassaggiPreparazione.Remove(passaggioToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        // -----------------------------------------------------------------
        // Metodo privato di mappatura entity -> DTO
        // -----------------------------------------------------------------
        private PassaggioPreparazioneDTO MapToDTO(PassaggioPreparazione p)
        {
            return new PassaggioPreparazioneDTO
            {
                Id = p.Id,
                Ordine = p.Ordine,
                Descrizione = p.Descrizione,
                RicettaId = p.RicettaId,
                Immagine = p.Immagine == null
                    ? null
                    : new ImmagineDTO
                    {
                        Id = p.Immagine.Id,
                        Url = p.Immagine.Url,
                        IsCover = p.Immagine.IsCover,
                        RicettaId = p.Immagine.RicettaId,
                        IngredienteId = p.Immagine.IngredienteId,
                        PassaggioPreparazioneId = p.Immagine.PassaggioPreparazioneId,
                        CategoriaId = p.Immagine.CategoriaId
                    }
            };
        }
    }
}
