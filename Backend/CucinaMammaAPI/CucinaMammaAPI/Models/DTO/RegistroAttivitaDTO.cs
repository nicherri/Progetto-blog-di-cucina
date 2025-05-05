using CucinaMammaAPI.Models;
using System;

namespace CucinaMammaAPI.DTOs
{
    public class RegistroAttivitaDTO
    {
        public int Id { get; set; } // ID dell'attività registrata

        public int UtenteId { get; set; } // ID dell'utente che ha eseguito l'azione
        public string NomeUtente { get; set; } // Nome dell'utente

        public TipoAzione Azione { get; set; } // Es: "Login", "Logout", "Creazione Ricetta", "Salvataggio Preferito"
        public string? Descrizione { get; set; } // Informazioni aggiuntive sull'operazione

        public DateTime Data { get; set; } // Timestamp dell'attività

        // 🔹 **Localizzazione e Sicurezza**
        public string? IndirizzoIP { get; set; } // Indirizzo IP dell'utente
        public string? Nazione { get; set; } // Nazione di provenienza
        public string? Citta { get; set; } // Città di provenienza
        public string? Dispositivo { get; set; } // Dispositivo utilizzato (es. "Windows 10", "iPhone 13")
        public string? Browser { get; set; } // Browser utilizzato (es. "Chrome 109", "Safari 15")

        public bool TentativoFallito { get; set; } // True se l'operazione è fallita (es. login errato)
        public bool AccessoSospetto { get; set; } // True se l'accesso è da una posizione insolita

        // 🔹 **Dati Opzionali per Azioni Specifiche**
        public int? RicettaId { get; set; } // ID della ricetta se l'azione riguarda una ricetta
        public string? NomeRicetta { get; set; } // Titolo della ricetta se presente
    }
}
