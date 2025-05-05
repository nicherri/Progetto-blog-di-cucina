using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IImmagineService
    {
        // 1) Lettura
        Task<IEnumerable<ImmagineDTO>> GetAllImmaginiAsync();
        Task<ImmagineDTO?> GetImmagineByIdAsync(int id);

        // 2) Caricamento fisico dei file (upload)
        Task<List<ImmagineDTO>> UploadImmaginiAsync(EntitaTipo tipo, int entitaId, List<IFormFile> files, List<ImmagineDTO>? metadati = null);

        // 3) Aggiornamento di metadati (es. IsCover)
        Task<ImmagineDTO?> UpdateImmagineAsync(int id, ImmagineDTO immagineDto);

        // 4) Cancellazione
        Task<bool> DeleteImmagineAsync(int id);
    }
}
