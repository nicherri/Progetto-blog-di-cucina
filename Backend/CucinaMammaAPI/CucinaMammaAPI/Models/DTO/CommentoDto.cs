using System;

namespace CucinaMammaAPI.DTOs
{
    public class CommentoDto
    {
        public int Id { get; set; }
        public string Testo { get; set; }
        public DateTime DataCreazione { get; set; }
        public int UtenteId { get; set; }
        public int RicettaId { get; set; }
    }
}
