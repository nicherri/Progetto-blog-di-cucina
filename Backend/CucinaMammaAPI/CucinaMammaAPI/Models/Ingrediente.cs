using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.Models
{
    public class Ingrediente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        // Relazione molti-a-molti con RicettaIngrediente
        public List<RicettaIngrediente> RicettaIngredienti { get; set; } = new();

        // Relazione con le immagini dell'ingrediente
        public List<Immagine> Immagini { get; set; } = new();
    }
}
