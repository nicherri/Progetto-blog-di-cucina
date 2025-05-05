namespace CucinaMammaAPI.DTOs
{
    public class ImmagineDTO
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool IsCover { get; set; } = false; // Indica se l'immagine è la copertina della ricetta o della categoria
        public int Ordine { get; set; } = 0; // Ordine di visualizzazione dell'immagine
        public string Alt { get; set; } = string.Empty;
        public string NomeFileSeo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        // Relazioni (solo gli ID per evitare cicli di riferimento)
        public int? RicettaId { get; set; }
        public int? IngredienteId { get; set; }
        public int? PassaggioPreparazioneId { get; set; }
        public int? CategoriaId { get; set; }
        public int? FatteDaVoiId { get; set; }
    }
}
