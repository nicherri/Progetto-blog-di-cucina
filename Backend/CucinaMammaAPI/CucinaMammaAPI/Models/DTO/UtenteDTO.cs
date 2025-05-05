using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class UtenteDTO
    {
        public int? Id { get; set; } // Nullable per compatibilità con Create

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(50)]
        public string Cognome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; } // Solo per input, in output non verrà mostrata

        public string? Ruolo { get; set; } = "Consumer";
    }
}
