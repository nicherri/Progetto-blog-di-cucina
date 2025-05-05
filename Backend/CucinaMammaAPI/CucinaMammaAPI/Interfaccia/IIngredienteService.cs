using CucinaMammaAPI.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IIngredienteService
    {
        Task<IEnumerable<IngredienteDTO>> GetAllIngredientiAsync();
        Task<IngredienteDTO?> GetIngredienteByIdAsync(int id);
        Task<IngredienteDTO> CreateIngredienteAsync(IngredienteDTO ingredienteDto);
        Task<IngredienteDTO?> UpdateIngredienteAsync(int id, IngredienteDTO ingredienteDto, List<IFormFile>? nuoveImmagini, List<int>? immaginiDaRimuovere);
        Task<bool> DeleteIngredienteAsync(int id);

        // 🟢 Gestione Relazione Ricetta-Ingrediente
        Task<IEnumerable<RicettaIngredienteDTO>> GetIngredientiByRicettaIdAsync(int ricettaId);
        Task<RicettaIngredienteDTO> AddIngredienteToRicettaAsync(RicettaIngredienteDTO ricettaIngredienteDto);
        Task<bool> RemoveIngredienteFromRicettaAsync(int ricettaId, int ingredienteId);
        Task<RicettaIngredienteDTO?> UpdateRicettaIngredienteAsync(int ricettaId, int ingredienteId, RicettaIngredienteDTO ricettaIngredienteDto);
    }
}
