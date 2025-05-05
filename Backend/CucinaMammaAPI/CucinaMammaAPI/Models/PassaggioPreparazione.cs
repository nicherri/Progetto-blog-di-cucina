using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class PassaggioPreparazione
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Ordine { get; set; } // Numero progressivo del passaggio

        [Required]
        [MaxLength(1000)]
        public string Descrizione { get; set; }

        // Relazione con la Ricetta
        public int RicettaId { get; set; }
        public Ricetta Ricetta { get; set; }

        // Relazione opzionale con un'Immagine (Se il passaggio ha un'immagine associata)
        public int? ImmagineId { get; set; }
        public Immagine? Immagine { get; set; }

        public List<Immagine> Immagini { get; set; } = new List<Immagine>();

    }
}
