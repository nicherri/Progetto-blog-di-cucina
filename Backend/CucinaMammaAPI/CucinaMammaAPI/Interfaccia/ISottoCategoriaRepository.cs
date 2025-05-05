using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Models;

namespace CucinaMammaAPI.Interfaces
{
    public interface ISottoCategoriaRepository
    {
        Task<List<SottoCategoriaDto>> GetAllSottoCategorieAsync();
        Task<SottoCategoriaDto?> GetSottoCategoriaByIdAsync(int id);
        Task<SottoCategoria> AddSottoCategoriaAsync(SottoCategoria sottoCategoria);
        Task<bool> UpdateSottoCategoriaAsync(int id, SottoCategoriaDto dto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere);
        Task<bool> DeleteSottoCategoriaAsync(int id);
        Task<bool> SottoCategoriaExistsAsync(int id);
    }
}
