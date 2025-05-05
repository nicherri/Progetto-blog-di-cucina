using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CucinaMammaAPI.Models
{
    public class Immagine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public bool IsCover { get; set; } = false;

        [MaxLength(255)]
        public string alt { get; set; } = ""; // ✅ NON Required ma default vuoto

        [MaxLength(255)]
        public string NomeFileSeo { get; set; } = ""; // ✅ default vuoto

        [MaxLength(255)]
        public string? Title { get; set; } = "";

        [MaxLength(500)]
        public string? Caption { get; set; } = "";

        public int Ordine { get; set; } = 0;

        // 🔹 Relazione opzionale con la Ricetta
        // Se la Ricetta viene eliminata, l'immagine verrà eliminata in cascata tramite il DbContext
        public int? RicettaId { get; set; }
        public Ricetta? Ricetta { get; set; }

        // 🔹 Relazione opzionale con un Ingrediente
        // Se l'ingrediente viene eliminato, l'immagine non viene eliminata automaticamente
        public int? IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }

        // 🔹 Relazione opzionale con un Passaggio di Preparazione
        // Se il passaggio viene eliminato, l'immagine non viene eliminata automaticamente
        public int? PassaggioPreparazioneId { get; set; }
        public PassaggioPreparazione? PassaggioPreparazione { get; set; }

        // 🔹 Relazione opzionale con una Categoria
        [JsonIgnore]
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        // 🔹 Relazione: Un'immagine può appartenere a UNA SottoCategoria
        public int? SottoCategoriaId { get; set; }
        public SottoCategoria? SottoCategoria { get; set; }

        // 🔹 Relazione: Un'immagine può appartenere a UNA sezione "Fatte da Voi"
        public int? FatteDaVoiId { get; set; }
        public FatteDaVoi? FatteDaVoi { get; set; }
    }
}
