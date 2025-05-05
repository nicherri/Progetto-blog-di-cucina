using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public class EventTracking
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Collegato a SessionTracking.SessionId. Necessario per raggruppare tutti gli
        /// eventi di una specifica sessione.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SessionId { get; set; } 


        /// <summary>
        /// Utente autenticato o anonimo (null). Può essere lo stesso salvato in SessionTracking.
        /// </summary>
        [MaxLength(100)]
        public string? UserId { get; set; }

        /// <summary>
        /// Nome o "label" dell’evento (es. "PageView", "RecipeClick", "AddToCart", "VideoPlay", ecc.).
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string EventName { get; set; }

        /// <summary>
        /// Categoria dell’evento (in stile Google Analytics),
        /// utile per raggruppare: ad esempio "Navigazione", "Interazione", "Acquisto", "Ricetta".
        /// </summary>
        [MaxLength(50)]
        public string? EventCategory { get; set; }

        /// <summary>
        /// Etichetta opzionale per fornire un contesto più specifico,
        /// es. "Clic su ingrediente" o "Video Ricetta Principale".
        /// </summary>
        [MaxLength(100)]
        public string? EventLabel { get; set; }

        /// <summary>
        /// Valore numerico associato all'evento (es. prezzo, quantità, punteggio, posizione).
        /// </summary>
        public decimal? EventValue { get; set; }

        /// <summary>
        /// URL della pagina (o della risorsa) su cui si è verificato l’evento.
        /// </summary>
        [MaxLength(500)]
        public string PageUrl { get; set; }

        /// <summary>
        /// Sito/pagina di provenienza (referrer).
        /// </summary>
        [MaxLength(500)]
        public string Referrer { get; set; }

        /// <summary>
        /// Timestamp UTC in cui si è verificato l’evento.
        /// </summary>
        public DateTime TimestampUtc { get; set; }


        /// <summary>
        /// Tempo (in secondi) che l’utente ha trascorso su questa pagina
        /// prima di passare alla successiva o prima di generare un nuovo evento.
        /// Spesso calcolato a posteriori (post-processing).
        /// </summary>
        public double? TimeSpentSeconds { get; set; }

        /// <summary>
        /// Percentuale di scroll della pagina raggiunta, se rilevata
        /// (0-100). Utile per capire l’engagement su articoli/ricette lunghe.
        /// </summary>
        public int? ScrollDepthPercentage { get; set; }

        /// <summary>
        /// JSON o stringa generica che può contenere metadati aggiuntivi:
        /// ad esempio l'ID di una ricetta, l'elenco ingredienti, la posizione dello scroll, ecc.
        /// Esempio: 
        /// { "RecipeId": 123, "ClickedIngredient": "Cipolla" }
        /// </summary>
        public string? AdditionalData { get; set; }


        /// <summary>
        /// Se l’utente ha scelto l’opt-out, potrebbe essere necessario filtrare
        /// o mascherare determinate informazioni su questo evento.
        /// </summary>
        public bool OptOut { get; set; }

        /// <summary>
        /// Relazione facoltativa (se usi EF) per agganciare all’entità SessionTracking,
        /// cosicché tu possa navigare dagli eventi alla sessione e viceversa.
        /// </summary>
        [ForeignKey(nameof(SessionId))]
        public virtual SessionTracking? Session { get; set; }

        /// <summary>
        /// Coordinate del mouse (es: click)
        /// </summary>
        public int? MouseX { get; set; }
        public int? MouseY { get; set; }

        /// <summary>
        /// Coordinate dello scroll al momento del click
        /// </summary>
        public int? ScrollX { get; set; }
        public int? ScrollY { get; set; }

        /// <summary>
        /// Dimensioni finestra (viewport)
        /// </summary>
        public int? ViewportWidth { get; set; }
        public int? ViewportHeight { get; set; }

        /// <summary>
        /// Bounding box dell'elemento cliccato (per heatmap)
        /// </summary>
        public int? ElementLeft { get; set; }
        public int? ElementTop { get; set; }
        public int? ElementWidth { get; set; }
        public int? ElementHeight { get; set; }

        /// <summary>
        /// Eventuale chunk di replay (es. mouse move, scroll, mutation),
        /// se vuoi comunque salvare un blocco di dati.
        /// </summary>
        public string? ReplayChunkData { get; set; }

        /// <summary>
        /// Tipo di chunk (mouse, scroll, mutation)
        /// </summary>
        public string? ReplayChunkType { get; set; }

        /// <summary>
        /// Se l'evento rappresenta uno step di funnel (AddToCart, Checkout, Payment),
        /// potresti mettere qui info come un ID ricetta, prezzo, ...
        /// </summary>
        [MaxLength(200)]
        public string? FunnelData { get; set; }

        public bool IsImported { get; set; }

        public string? FunnelStep { get; set; }

    
    }
}
