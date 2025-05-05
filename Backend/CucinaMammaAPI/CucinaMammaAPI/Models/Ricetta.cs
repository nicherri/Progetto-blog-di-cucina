using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace CucinaMammaAPI.Models
{
    public class Ricetta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titolo { get; set; } = string.Empty;

        public string Descrizione { get; set; } = string.Empty;
        [Required]

        public int TempoPreparazione { get; set; } // Tempo in minuti

        public string Difficolta { get; set; } // Facile, Media, Difficile
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public DateTime DataCreazione { get; set; } = DateTime.UtcNow;

        // 🔹Proprietà per la pubblicazione programmata
        public DateTime? DataPubblicazione { get; set; } // Se null, la ricetta non è pubblicata

        // 🔹 Proprietà calcolata per determinare se la ricetta è pubblicata
        public bool Published => DataPubblicazione.HasValue && DataPubblicazione <= DateTime.UtcNow;


        // Relazioni
        [Required]
        public List<RicettaIngrediente> RicettaIngredienti { get; set; } = new();
        public List<Immagine> Immagini { get; set; } = new List<Immagine>();
        public List<RicettaCategoria> RicetteCategorie { get; set; } = new();
        public List<RicettaSottoCategoria> RicetteSottoCategorie { get; set; } = new(); // Nuova relazione

        [Required]
        public List<PassaggioPreparazione> PassaggiPreparazione { get; set; } = new(); // Nuova relazione

        // Proprietà opzionale per autore (futuro)
        public int? UtenteId { get; set; }
        public Utente? Utente { get; set; }

        public List<RicettaPreferita> SalvataDaUtenti { get; set; } = new();
        public List<RegistroAttivita> RegistriAttivita { get; set; } = new();

        //  Commenti degli utenti
        public List<Commento> Commenti { get; set; } = new();

        // Immagini "Fatte da voi"
        public List<FatteDaVoi> ImmaginiFatteDaVoi { get; set; } = new();
    }
}
