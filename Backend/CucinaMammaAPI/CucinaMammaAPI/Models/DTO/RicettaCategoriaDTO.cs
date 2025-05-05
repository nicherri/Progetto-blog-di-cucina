namespace CucinaMammaAPI.DTOs
{
    public class RicettaCategoriaDTO
    {
        public int RicettaId { get; set; } // ID della ricetta
        public string Titolo { get; set; } // Nome della ricetta
        public string? ImmagineCopertina { get; set; } // URL della cover della ricetta
    }
}
