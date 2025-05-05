using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class RicettaDTO
    {
        public int? Id { get; set; } // Nullable per permettere il riutilizzo nel Create

        [Required]
        [MaxLength(100)]
        public string Titolo { get; set; }

        public string Descrizione { get; set; }
        [Required]
        public int TempoPreparazione { get; set; }
        public bool Published { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public DateTime? DataPubblicazione { get; set; }

        [Required]
        public string Difficolta { get; set; } // Facile, Media, Difficile

        [Required]
       
        public List<CategoriaDTO> Categorie { get; set; } = new();
        [Required]
        public List<PassaggioPreparazioneDTO> PassaggiPreparazione { get; set; } = new();

        public DateTime? DataCreazione { get; set; } = DateTime.UtcNow;

        // Relazioni
        public List<RicettaIngredienteDTO> RicettaIngredienti { get; set; } = new();
        public List<ImmagineDTO> Immagini { get; set; } = new();
        public List<RicettaSottoCategoriaDto> RicetteSottoCategorie { get; set; }
        public List<CommentoDto> Commenti { get; set; }
        public List<FatteDaVoiDto> ImmaginiFatteDaVoi { get; set; }

        // Info autore
        public int? UtenteId { get; set; }
        public string? AutoreNome { get; set; }
    }
}
