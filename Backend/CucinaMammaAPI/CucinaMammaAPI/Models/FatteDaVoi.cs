using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class FatteDaVoi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int UtenteId { get; set; }
        public Utente Utente { get; set; }

        [Required]
        public int RicettaId { get; set; }
        public Ricetta Ricetta { get; set; }

        public DateTime DataCaricamento { get; set; } = DateTime.UtcNow;
        public List<Immagine> Immagini { get; set; } = new();

    }
}
