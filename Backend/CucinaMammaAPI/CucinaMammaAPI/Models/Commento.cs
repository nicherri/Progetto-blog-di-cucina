using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class Commento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Testo { get; set; }

        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        // Relazione con l'utente che ha scritto il commento
        [Required]
        public int UtenteId { get; set; }
        public Utente Utente { get; set; }

        // Relazione con la ricetta commentata
        [Required]
        public int RicettaId { get; set; }
        public Ricetta Ricetta { get; set; }
    }
}
