namespace CucinaMammaAPI.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descrizione { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; // 🔹 Usato per URL SEO-friendly

        public string? SeoTitle { get; set; }            // 🔹 Per <title> HTML
        public string? SeoDescription { get; set; }      // 🔹 Per <meta name="description">

        public List<ImmagineDTO> Immagini { get; set; } = new List<ImmagineDTO>();

        public List<RicettaCategoriaDTO> Ricette { get; set; } = new();

        public List<CategoriaSottoCategoriaDto>? CategorieSottoCategorie { get; set; } = new();


        public IFormFile? ImmagineFile { get; set; }


    }
}
