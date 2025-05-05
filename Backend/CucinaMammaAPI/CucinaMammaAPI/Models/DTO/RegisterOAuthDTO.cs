using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class RegisterOAuthDTO
    {
        [Required]
        public string Provider { get; set; } // "Google" o "Apple"

        [Required]
        public string ProviderId { get; set; } // ID restituito da Google/Apple

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        public string? FotoProfiloUrl { get; set; } // URL della foto profilo

        public bool EmailVerificata { get; set; } = false; // True se il provider ha verificato l'email
    }
}
