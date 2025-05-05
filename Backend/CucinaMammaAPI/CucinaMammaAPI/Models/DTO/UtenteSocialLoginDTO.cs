namespace CucinaMammaAPI.DTOs
{
    public class UtenteSocialLoginDTO
    {
        public int Id { get; set; }
        public int UtenteId { get; set; }
        public string Provider { get; set; } // "Google" o "Apple"
        public string ProviderId { get; set; } // ID univoco Google/Apple
        public DateTime DataCollegamento { get; set; }
    }
}
