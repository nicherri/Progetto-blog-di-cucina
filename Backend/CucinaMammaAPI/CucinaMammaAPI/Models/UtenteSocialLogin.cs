using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class UtenteSocialLogin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UtenteId { get; set; }
        public Utente Utente { get; set; }

        [Required]
        public string Provider { get; set; } // Es: "Google" o "Apple"

        [Required]
        public string ProviderId { get; set; } // ID univoco restituito da Google/Apple

        public DateTime DataCollegamento { get; set; } = DateTime.UtcNow;

        public bool Attivo { get; set; } = true;
    }
}
