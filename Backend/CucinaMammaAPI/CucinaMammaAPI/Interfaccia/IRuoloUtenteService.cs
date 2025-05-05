using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IRuoloUtenteService
    {
        // 🔹 Recupera tutti i ruoli disponibili
        List<RuoloUtenteDTO> GetAllRuoli();

        // 🔹 Converte un enum RuoloUtente in stringa leggibile
        string GetRuoloNome(RuoloUtente ruolo);

        // 🔹 Converte una stringa in enum RuoloUtente
        RuoloUtente? GetRuoloFromString(string nomeRuolo);

        // 🔹 Assegna un nuovo ruolo a un utente
        Task<bool> AssegnaRuoloUtenteAsync(int utenteId, RuoloUtente nuovoRuolo);
    }
}
