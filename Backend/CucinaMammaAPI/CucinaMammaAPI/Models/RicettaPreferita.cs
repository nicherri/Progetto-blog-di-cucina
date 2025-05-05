using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class RicettaPreferita
    {
        [Key]
        public int Id { get; set; }

        // 🔹 Relazione con l'utente che ha salvato la ricetta
        [Required]
        [ForeignKey("Utente")]
        public int UtenteId { get; set; }
        public Utente Utente { get; set; }

        // 🔹 Relazione con la ricetta salvata
        [Required]
        [ForeignKey("Ricetta")]
        public int RicettaId { get; set; }
        public Ricetta Ricetta { get; set; }

        public DateTime DataSalvataggio { get; set; } = DateTime.UtcNow;
    }
}
