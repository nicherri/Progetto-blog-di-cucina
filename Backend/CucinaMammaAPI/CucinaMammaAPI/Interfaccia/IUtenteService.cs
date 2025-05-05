using CucinaMammaAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IUtenteService
    {
        // 🔹 Recupera tutti gli utenti con paginazione
        Task<IEnumerable<UtenteDTO>> GetAllUtentiAsync(int pageIndex = 0, int pageSize = 10);

        // 🔹 Recupera un utente per ID
        Task<UtenteDTO?> GetUtenteByIdAsync(int id);

        // 🔹 Recupera un utente per email
        Task<UtenteDTO?> GetUtenteByEmailAsync(string email);

        // 🔹 Modifica le informazioni di un utente
        Task<UtenteDTO?> UpdateUtenteAsync(int id, UtenteDTO utenteDto);

        // 🔹 Elimina un utente
        Task<bool> DeleteUtenteAsync(int id);
    }
}
