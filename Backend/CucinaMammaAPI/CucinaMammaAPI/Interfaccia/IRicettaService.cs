using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Services
{
    public interface IRicettaService
    {
        Task<IEnumerable<RicettaDTO>> GetAllRicetteAsync(); // Recupera tutte le ricette
        Task<RicettaDTO?> GetRicettaByIdAsync(int id); // Recupera una ricetta per ID
        Task<RicettaDTO> CreateRicettaAsync(RicettaDTO ricettaDto); // Crea una nuova ricetta
        Task<RicettaDTO?> UpdateRicettaAsync(int id, RicettaDTO ricettaDto); // Modifica una ricetta
        Task<bool> DeleteRicettaAsync(int id); // Elimina una ricetta
    }
}
