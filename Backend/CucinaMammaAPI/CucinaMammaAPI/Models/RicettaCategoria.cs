using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class RicettaCategoria
    {
        public int RicettaId { get; set; }
        [ForeignKey("RicettaId")]
        public Ricetta Ricetta { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}
