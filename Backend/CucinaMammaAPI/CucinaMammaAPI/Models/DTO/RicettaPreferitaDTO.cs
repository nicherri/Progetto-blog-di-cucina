using System;
using System.ComponentModel.DataAnnotations;

namespace CucinaMammaAPI.DTOs
{
    public class RicettaPreferitaDTO
    {
        public int? Id { get; set; } // Nullable per compatibilità con Create

        [Required]
        public int RicettaId { get; set; }

        public string? TitoloRicetta { get; set; } // Popolato in output

        public int UtenteId { get; set; }

        public DateTime? DataSalvataggio { get; set; } = DateTime.UtcNow;
    }
}
