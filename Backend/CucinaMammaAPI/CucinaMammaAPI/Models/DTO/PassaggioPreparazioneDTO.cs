namespace CucinaMammaAPI.DTOs
{
    public class PassaggioPreparazioneDTO
    {
        public int Id { get; set; }
        public int Ordine { get; set; } // Numero del passaggio nella sequenza
        public string Descrizione { get; set; }

        // Relazioni
        public int RicettaId { get; set; }
        public ImmagineDTO? Immagine { get; set; } // L'immagine associata al passaggio
    }
}
