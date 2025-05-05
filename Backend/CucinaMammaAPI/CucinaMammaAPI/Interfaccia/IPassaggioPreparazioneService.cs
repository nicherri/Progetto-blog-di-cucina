using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Services
{
    public interface IPassaggioPreparazioneService
    {
        Task<List<PassaggioPreparazioneDTO>> GetAllAsync(); // Ottenere tutti i passaggi di preparazione
        Task<List<PassaggioPreparazioneDTO>> GetByRicettaIdAsync(int ricettaId); // Ottenere tutti i passaggi di una ricetta
        Task<PassaggioPreparazioneDTO?> GetByIdAsync(int id); // Ottenere un singolo passaggio per ID
        Task<PassaggioPreparazioneDTO> CreateAsync(PassaggioPreparazioneDTO passaggioDto); // Creare un nuovo passaggio
        Task<bool> UpdateAsync(int id, PassaggioPreparazioneDTO passaggioDto); // Modificare un passaggio esistente
        Task<bool> DeleteAsync(int id); // Eliminare un passaggio
    }
}
