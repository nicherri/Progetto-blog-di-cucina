namespace CucinaMammaAPI.Models
{
    public enum RuoloUtente
    {
        Admin,       // Può gestire tutto il sito
        Proprietario, // Può inserire ricette (es. tua mamma)
        Consumer     // Può solo salvare ricette preferite
    }
}
