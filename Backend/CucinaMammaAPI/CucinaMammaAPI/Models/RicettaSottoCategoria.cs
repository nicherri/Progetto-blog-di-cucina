using System.ComponentModel.DataAnnotations.Schema;

namespace CucinaMammaAPI.Models
{
    public class RicettaSottoCategoria
    {
        public int RicettaId { get; set; }
        [ForeignKey("RicettaId")]
        public Ricetta Ricetta { get; set; }

        public int SottoCategoriaId { get; set; }
        [ForeignKey("SottoCategoriaId")]
        public SottoCategoria SottoCategoria { get; set; }
    }
}
