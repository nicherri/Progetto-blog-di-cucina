using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class RicettaIngrediente
    {
        [Key]
        public int Id { get; set; }

        public int RicettaId { get; set; }
        public Ricetta Ricetta { get; set; }

        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; }

        [Required]
        public int Quantita { get; set; } // Es: 100

        [Required]
        [MaxLength(50)]
        public string UnitaMisura { get; set; } // Es: g, ml, cucchiai
    }
}
