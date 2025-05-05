using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.Models
{
    public class ThemeSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Component { get; set; } // Esempio: "header", "footer", "button"

        [Required]
        public string Property { get; set; }  // Esempio: "color", "font-size", "background"

        [Required]
        public string Value { get; set; }     // Esempio: "#ff9800", "16px", "Roboto"
    }
}
