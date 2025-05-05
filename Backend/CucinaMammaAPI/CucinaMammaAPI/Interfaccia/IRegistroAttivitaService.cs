using CucinaMammaAPI.DTOs;
using CucinaMammaAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IRegistroAttivitaService
    {
        // 🔹 Recupera tutte le attività con supporto per paginazione e filtri
        Task<IEnumerable<RegistroAttivitaDTO>> GetAllAttivitaAsync(
            int pageIndex = 0,
            int pageSize = 10,
            TipoAzione? azione = null,
            int? utenteId = null,
            DateTime? dataInizio = null,
            DateTime? dataFine = null);

        // 🔹 Recupera un'attività per ID
        Task<RegistroAttivitaDTO?> GetAttivitaByIdAsync(int id);

        // 🔹 Registra una nuova attività
        Task<RegistroAttivitaDTO> RegistraAttivitaAsync(
            int utenteId,
            TipoAzione azione,
            string? descrizione = null,
            string? indirizzoIP = null,
            string? nazione = null,
            string? citta = null,
            string? dispositivo = null,
            string? browser = null,
            bool tentativoFallito = false,
            bool accessoSospetto = false,
            int? ricettaId = null,
            string? nomeRicetta = null);

        // 🔹 Elimina un'attività per ID
        Task<bool> DeleteAttivitaAsync(int id);

        // 🔹 Recupera tutte le attività di un determinato utente
        Task<IEnumerable<RegistroAttivitaDTO>> GetAttivitaByUtenteIdAsync(int utenteId);

        // 🔹 Recupera tutte le attività di un certo tipo
        Task<IEnumerable<RegistroAttivitaDTO>> GetAttivitaByAzioneAsync(TipoAzione azione);

        // 🔹 Recupera le attività in un determinato intervallo temporale
        Task<IEnumerable<RegistroAttivitaDTO>> GetAttivitaByDateRangeAsync(DateTime dataInizio, DateTime dataFine);
    }
}
