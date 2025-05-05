using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class CategoriaSottoCategoria
    {
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        public int SottoCategoriaId { get; set; }
        [ForeignKey("SottoCategoriaId")]
        public SottoCategoria SottoCategoria { get; set; }
    }
}
