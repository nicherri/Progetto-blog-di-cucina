using CucinaMammaAPI.Data;
using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using CucinaMammaAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace CucinaMammaAPI.Services
{
    public class ImmagineService : IImmagineService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        // Limite di dimensione (5 MB, per esempio)
        private const long MAX_FILE_SIZE = 5L * 1024L * 1024L;

        public ImmagineService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        #region (1) Lettura

        /// <summary>
        /// Restituisce tutte le immagini nel DB.
        /// </summary>
        public async Task<IEnumerable<ImmagineDTO>> GetAllImmaginiAsync()
        {
            var immagini = await _context.Immagini.ToListAsync();
            return immagini.Select(MappaImmagineToDTO);
        }

        /// <summary>
        /// Restituisce i dati di una singola immagine, oppure null se non esiste.
        /// </summary>
        public async Task<ImmagineDTO?> GetImmagineByIdAsync(int id)
        {
            var immagine = await _context.Immagini.FindAsync(id);
            if (immagine == null)
                return null;

            return MappaImmagineToDTO(immagine);
        }

        #endregion

        #region (2) Upload multiplo di file

        /// <summary>
        /// Carica una lista di file (IFormFile) per una determinata entità (ricetta, categoria, etc.).
        /// Ogni file viene salvato fisicamente in wwwroot/uploads/..., e un record in DB Immagini.
        /// </summary>
        public async Task<List<ImmagineDTO>> UploadImmaginiAsync(
    EntitaTipo tipo,
    int entitaId,
    List<IFormFile> files,
    List<ImmagineDTO>? metadati = null)
        {
            var result = new List<ImmagineDTO>();
            var cartellaFisica = CostruisciPercorsoCartella(tipo, entitaId);
            Directory.CreateDirectory(cartellaFisica);

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                if (file.Length == 0 || file.Length > MAX_FILE_SIZE)
                {
                    Console.WriteLine($"❌ File {file.FileName} è vuoto o troppo grande.");
                    continue;
                }

                var estensione = Path.GetExtension(file.FileName);
                var uniqueName = $"{Guid.NewGuid()}{estensione}";
                var filePath = Path.Combine(cartellaFisica, uniqueName);

                try
                {
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    var relativeUrl = CostruisciUrlRelativo(tipo, entitaId, uniqueName);
                    var meta = metadati?.ElementAtOrDefault(i);

                    Console.WriteLine($"🟡 Metadata per file {i}:");
                    Console.WriteLine($"   Alt: {meta?.Alt}");
                    Console.WriteLine($"   Caption: {meta?.Caption}");
                    Console.WriteLine($"   Title: {meta?.Title}");
                    Console.WriteLine($"   NomeFileSeo: {meta?.NomeFileSeo}");
                    Console.WriteLine($"   IsCover: {meta?.IsCover}");
                    Console.WriteLine($"   Ordine: {meta?.Ordine}");

                    var immagine = new Immagine
                    {
                        Url = relativeUrl,
                        CategoriaId = tipo == EntitaTipo.Categoria ? entitaId : null,
                        RicettaId = tipo == EntitaTipo.Ricetta ? entitaId : null,
                        IngredienteId = tipo == EntitaTipo.Ingrediente ? entitaId : null,
                        IsCover = meta?.IsCover ?? false,
                        alt = meta?.Alt ?? "Default alt",
                        Caption = meta?.Caption ?? "Default caption",
                        NomeFileSeo = meta?.NomeFileSeo ?? Path.GetFileNameWithoutExtension(file.FileName),
                        Title = meta?.Title ?? "Titolo immagine",
                        Ordine = meta?.Ordine ?? 0
                    };

                    _context.Immagini.Add(immagine);
                    await _context.SaveChangesAsync();

                    result.Add(new ImmagineDTO
                    {
                        Id = immagine.Id,
                        Url = immagine.Url,
                        IsCover = immagine.IsCover,
                        CategoriaId = immagine.CategoriaId,
                        RicettaId = immagine.RicettaId,
                        IngredienteId = immagine.IngredienteId,
                        Alt = immagine.alt,
                        Caption = immagine.Caption,
                        NomeFileSeo = immagine.NomeFileSeo,
                        Title = immagine.Title,
                        Ordine = immagine.Ordine
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Errore durante il salvataggio dell'immagine {file.FileName}: {ex.Message}");
                }
            }

            return result;
        }

        #endregion

        #region (3) Update metadati

        /// <summary>
        /// Aggiorna i metadati di un'immagine (es. IsCover, Alt, Title, Caption, NomeFileSeo, Ordine)
        /// </summary>
        public async Task<ImmagineDTO?> UpdateImmagineAsync(int id, ImmagineDTO immagineDto)
        {
            var immagine = await _context.Immagini.FindAsync(id);
            if (immagine == null)
                return null;

            // 🔄 Aggiorna tutti i metadati definiti nel model
            immagine.IsCover = immagineDto.IsCover;
            immagine.alt = immagineDto.Alt?.Trim() ?? "";
            immagine.NomeFileSeo = immagineDto.NomeFileSeo?.Trim() ?? "";
            immagine.Title = immagineDto.Title?.Trim() ?? "";
            immagine.Caption = immagineDto.Caption?.Trim() ?? "";
            immagine.Ordine = immagineDto.Ordine;

            // 🔒 NON aggiorniamo l’URL del file fisico per evitare incoerenze
            // 🔒 NON aggiorniamo le FK (RicettaId, CategoriaId, ecc.) per sicurezza

            await _context.SaveChangesAsync();

            return MappaImmagineToDTO(immagine);
        }

        #endregion


        #region (4) Cancellazione

        /// <summary>
        /// Elimina il file fisico e rimuove il record Immagine dal DB.
        /// </summary>
        public async Task<bool> DeleteImmagineAsync(int id)
        {
            var immagine = await _context.Immagini.FindAsync(id);
            if (immagine == null)
                return false;

            // Elimina il file fisico (se esiste)
            var physicalPath = Path.Combine(_env.WebRootPath, immagine.Url.TrimStart('/'));
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }

            // Elimina dal DB
            _context.Immagini.Remove(immagine);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Helpers privati

        /// <summary>
        /// Costruisce la cartella fisica ("C:\...\wwwroot\uploads\ricette\{id}" ad esempio)
        /// in base al tipo di entità e all'ID.
        /// </summary>
        private string CostruisciPercorsoCartella(EntitaTipo tipo, int entitaId)
        {
            var root = _env.WebRootPath; // es: "C:\Progetti\CucinaMamma\wwwroot"

            if (string.IsNullOrEmpty(root))
            {
                throw new InvalidOperationException("WebRootPath non è stato impostato correttamente.");
            }

            switch (tipo)
            {
                case EntitaTipo.Ricetta:
                    return Path.Combine(root, "uploads", "ricette", entitaId.ToString());
                case EntitaTipo.Categoria:
                    return Path.Combine(root, "uploads", "categorie", entitaId.ToString());
                case EntitaTipo.Ingrediente:
                    return Path.Combine(root, "uploads", "ingredienti", entitaId.ToString());
                case EntitaTipo.Passaggio:
                    return Path.Combine(root, "uploads", "passaggi", entitaId.ToString());
                default:
                    return Path.Combine(root, "uploads", "varie", entitaId.ToString());
            }
        }

        /// <summary>
        /// Restituisce un percorso relativo ("/uploads/ricette/{id}/filename") da salvare in DB.
        /// </summary>
        private string CostruisciUrlRelativo(EntitaTipo tipo, int entitaId, string fileName)
        {
            switch (tipo)
            {
                case EntitaTipo.Ricetta:
                    return $"/uploads/ricette/{entitaId}/{fileName}";
                case EntitaTipo.Categoria:
                    return $"/uploads/categorie/{entitaId}/{fileName}";
                case EntitaTipo.Ingrediente:
                    return $"/uploads/ingredienti/{entitaId}/{fileName}";
                case EntitaTipo.Passaggio:
                    return $"/uploads/passaggi/{entitaId}/{fileName}";
                default:
                    return $"/uploads/varie/{entitaId}/{fileName}";
            }
        }

        /// <summary>
        /// Converte l'entità Immagine in un DTO, per restituire i dati al client.
        /// </summary>
        private ImmagineDTO MappaImmagineToDTO(Immagine img)
        {
            return new ImmagineDTO
            {
                Id = img.Id,
                Url = img.Url,
                IsCover = img.IsCover,
                RicettaId = img.RicettaId,
                CategoriaId = img.CategoriaId,
                IngredienteId = img.IngredienteId,
                PassaggioPreparazioneId = img.PassaggioPreparazioneId
            };
        }

        #endregion
    }
}
