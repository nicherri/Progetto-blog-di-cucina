using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class Utente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(50)]
        public string Cognome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PasswordHash { get; set; } // Null se l'utente usa solo Google o Apple

        [Required]
        public RuoloUtente Ruolo { get; set; } = RuoloUtente.Consumer; // Default: Consumer

        // 🔹 **Autenticazione via Google o Apple**
        public string? GoogleId { get; set; } // ID univoco Google
        public string? AppleId { get; set; } // ID univoco Apple

        public string? FotoProfiloUrl { get; set; } // URL foto profilo (se disponibile)

        // 🔹 **Gestione Sicurezza e Ultimo Accesso**
        public DateTime? UltimoAccesso { get; set; }
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        public string? RefreshToken { get; set; } // Token per il refresh della sessione
        public DateTime? RefreshTokenScadenza { get; set; } // Scadenza del refresh token

        // 🔹 **Email verificata**
        public bool EmailVerificata { get; set; } = false;

        // Relazioni con altre tabelle
        public List<Ricetta> RicetteCreate { get; set; } = new();
        public List<RicettaPreferita> RicettePreferite { get; set; } = new();
        public List<RegistroAttivita> RegistriAttivita { get; set; } = new();
    }
}
