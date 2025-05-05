using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CucinaMammaAPI.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } // Nome della categoria (es. "Primi Piatti", "Dolci", "Vegetariano")

        public string? Descrizione { get; set; } // Descrizione opzionale della categoria
        // 🔸 SEO Slug (es. "primi-piatti" usato negli URL)
        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        // 🔸 Meta tag SEO: <title>
        [MaxLength(70)] // Google taglia oltre 60–70 caratteri
        public string? SeoTitle { get; set; }

        // 🔸 Meta tag SEO: <meta name="description">
        [MaxLength(160)] // Lunghezza ideale secondo Google
        public string? SeoDescription { get; set; }

        // 🔹 Una categoria può avere più immagini
        public List<Immagine> Immagini { get; set; } = new List<Immagine>();

        // 🔹 Relazione molti-a-molti con Ricetta
        public List<RicettaCategoria> RicetteCategorie { get; set; } = new();

        // 🔹 Relazione: Una categoria può avere più sottocategorie
        public List<CategoriaSottoCategoria> CategorieSottoCategorie { get; set; } = new();
    }
}
