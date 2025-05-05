using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class RegistroAttivita
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UtenteId { get; set; }
        public Utente Utente { get; set; }

        [Required]
        public TipoAzione Azione { get; set; } // Es: "Login", "Logout", "Creazione Ricetta", "Salvataggio Preferito"

        public string? Descrizione { get; set; } // Descrizione dettagliata dell'azione

        [Required]
        public DateTime Data { get; set; } = DateTime.UtcNow; // Sempre UTC per coerenza nei fusi orari

        // 🔹 **Localizzazione e Sicurezza**
        public string? IndirizzoIP { get; set; } // IP dell'utente
        public string? Nazione { get; set; } // Es: "Italia"
        public string? Citta { get; set; } // Es: "Roma"
        public string? Dispositivo { get; set; } // Es: "Windows 10", "iPhone 13"
        public string? Browser { get; set; } // Es: "Chrome 109", "Safari 15"

        // 🔹 **Flag di sicurezza**
        public bool TentativoFallito { get; set; } = false; // Es: login errato
        public bool AccessoSospetto { get; set; } = false; // Es: login da posizione insolita

        // 🔹 **Dati Opzionali per Azioni Specifiche**
        public int? RicettaId { get; set; } // Se l'azione riguarda una ricetta
        public string? NomeRicetta { get; set; } // Titolo della ricetta se presente

        // 🔹 **Metodo ToString()**
        public override string ToString()
        {
            return $"{Data}: {Azione} da {IndirizzoIP} ({Nazione}, {Citta}) - Dispositivo: {Dispositivo}, Browser: {Browser} (TentativoFallito: {TentativoFallito}, AccessoSospetto: {AccessoSospetto})";
        }
    }
}
