using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CucinaMammaAPI.Models
{
    public class SottoCategoria
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nome { get; set; } // Es. "Pasta Ripiena", "Torte al Cioccolato"

        public string? Descrizione { get; set; }

        // Relazione con Categoria principale (ogni sottocategoria appartiene a una categoria)
        [Required]
        public List<CategoriaSottoCategoria> CategorieSottoCategorie { get; set; } = new();



        // Relazione con Ricette tramite RicettaSottoCategoria
        public List<RicettaSottoCategoria> RicetteSottoCategorie { get; set; } = new();

        public List<Immagine> Immagini { get; set; } = new();

    }
}
