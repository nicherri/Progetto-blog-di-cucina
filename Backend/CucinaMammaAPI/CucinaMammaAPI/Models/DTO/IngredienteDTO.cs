using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class IngredienteDTO
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        // Lista di ricette associate all'ingrediente con quantità e unità di misura
        public List<RicettaIngredienteDTO> RicettaIngredienti { get; set; } = new();
        public List<ImmagineDTO> Immagini { get; set; } = new();


        // Lista di immagini associate all'ingrediente
        public List<IFormFile> FileImmagini { get; set; } = new();
    }
}
