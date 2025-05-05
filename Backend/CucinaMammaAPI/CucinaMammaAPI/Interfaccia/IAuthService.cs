using CucinaMammaAPI.DTOs;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IAuthService
    {
        // 🔹 Login con email/password → restituisce JWT Token
        Task<string?> LoginAsync(string email, string password);

        // 🔹 Login tramite OAuth (Google/Apple) → restituisce JWT Token
        Task<string?> LoginOAuthAsync(LoginOAuthDTO loginOAuthDto);

        // 🔹 Registra un nuovo utente con email/password
        Task<UtenteDTO> RegistraUtenteAsync(UtenteDTO utenteDto);

        // 🔹 Registra un nuovo utente tramite OAuth
        Task<UtenteDTO> RegistraUtenteOAuthAsync(RegisterOAuthDTO registerOAuthDto);

        // 🔹 Reimposta la password se dimenticata
        Task<bool> ResetPasswordAsync(string email, string nuovaPassword);

        // 🔹 Cambia la password dell’utente autenticato
        Task<bool> CambiaPasswordAsync(int id, string vecchiaPassword, string nuovaPassword);

        // 🔹 Genera un nuovo access token con un refresh token
        Task<string?> RefreshTokenAsync(string refreshToken);

        // 🔹 Logout e invalidazione dei token
        Task<bool> LogoutAsync(int utenteId);
    }
}
