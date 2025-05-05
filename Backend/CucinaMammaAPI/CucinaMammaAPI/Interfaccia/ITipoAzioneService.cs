using CucinaMammaAPI.Models;
using CucinaMammaAPI.Models.DTO;
using System.Collections.Generic;

namespace CucinaMammaAPI.Services
{
    public interface ITipoAzioneService
    {
        List<TipoAzioneDTO> GetAllAzioni(); // Recupera la lista di tutte le azioni disponibili
        string GetAzioneNome(TipoAzione azione); // Converte un enum in una stringa leggibile
        TipoAzione? GetAzioneFromString(string nomeAzione); // Converte una stringa in un enum (se esiste)
    }
}
