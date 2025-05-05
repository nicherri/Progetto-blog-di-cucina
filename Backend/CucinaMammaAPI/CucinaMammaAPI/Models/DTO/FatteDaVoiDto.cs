using System;
using System.Collections.Generic;

namespace CucinaMammaAPI.DTOs
{
    public class FatteDaVoiDto
    {
        public int Id { get; set; }
        public int UtenteId { get; set; }
        public int RicettaId { get; set; }
        public DateTime DataCaricamento { get; set; }
        public List<ImmagineDTO> Immagini { get; set; } = new List<ImmagineDTO>();
        public IFormFile? ImmagineFile { get; set; }

    }
}
