namespace CucinaMammaAPI.DTOs
{
    public class RicettaIngredienteDTO
    {
        public int RicettaId { get; set; }
        public int IngredienteId { get; set; }
        public int Quantita { get; set; }
        public string UnitaMisura { get; set; }
    }
}
