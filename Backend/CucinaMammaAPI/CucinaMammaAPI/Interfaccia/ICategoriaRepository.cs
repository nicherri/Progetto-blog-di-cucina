using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Models;

namespace CucinaMammaAPI.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaDTO>> GetAllCategorieAsync();
        // Ottiene tutte le categorie
        Task<CategoriaDTO?> GetCategoriaByIdAsync(int id); // 🔹 Cambiato da Categoria? a CategoriaDTO?
        Task<List<RicettaDTO>> GetRicetteByCategoriaIdAsync(int categoriaId); // Ottiene tutte le ricette di una categoria

        Task<Categoria> AddCategoriaAsync(Categoria categoria); // Aggiunge una nuova categoria
        Task<bool> UpdateCategoriaAsync(int id, CategoriaDTO categoriaDto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere);
        Task<bool> DeleteCategoriaAsync(int id); // Elimina una categoria
        Task<bool> CategoriaExistsAsync(int id); // Controlla se una categoria esiste

        // 🔹 Relazioni tra Ricetta e Categoria
        Task<List<CategoriaDTO>> GetCategorieByRicettaIdAsync(int ricettaId); // Ottiene tutte le categorie di una ricetta
        Task<bool> AddRicettaToCategoriaAsync(int ricettaId, int categoriaId); // Aggiunge una ricetta a una categoria
        Task<bool> RemoveRicettaFromCategoriaAsync(int ricettaId, int categoriaId); // Rimuove una ricetta da una categoria

        // 🔹 Relazioni tra Ricetta e sottocategoria
        // 
        Task<List<SottoCategoriaDto>> GetSottoCategorieByCategoriaIdAsync(int categoriaId);
        Task<List<CategoriaDTO>> GetCategorieBySottoCategoriaIdAsync(int sottoCategoriaId);
        Task<bool> AddSottoCategoriaToCategoriaAsync(int categoriaId, int sottoCategoriaId);
        Task<bool> RemoveSottoCategoriaFromCategoriaAsync(int categoriaId, int sottoCategoriaId);
        Task<bool> CategoriaSottoCategoriaExistsAsync(int categoriaId, int sottoCategoriaId);
    }
}
