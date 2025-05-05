using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class LoginOAuthDTO
    {
        [Required]
        public string Provider { get; set; } // "Google" o "Apple"

        [Required]
        public string ProviderId { get; set; } // ID restituito da Google/Apple

        public string? Email { get; set; }
    }
}
