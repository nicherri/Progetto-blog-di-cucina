using System.Collections.Generic;

namespace CucinaMammaAPI.DTOs
{
    public class SottoCategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descrizione { get; set; }
        public List<ImmagineDTO> Immagini { get; set; } = new List<ImmagineDTO>();


        // 🔹 Nuova relazione molti-a-molti
        public List<CategoriaSottoCategoriaDto> CategorieSottoCategorie { get; set; }
        public IFormFile? ImmagineFile { get; set; }
    }
}
