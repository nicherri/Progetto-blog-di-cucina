using CucinaMammaAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    public interface IUtenteSocialLoginService
    {
        // 🔹 Recupera tutti gli account social collegati a un utente
        Task<IEnumerable<UtenteSocialLoginDTO>> GetSocialLoginsByUtenteIdAsync(int utenteId);

        // 🔹 Verifica se un utente ha già collegato un provider (Google/Apple)
        Task<bool> VerificaCollegamentoAsync(int utenteId, string provider);

        // 🔹 Collega un nuovo account social a un utente esistente
        Task<UtenteSocialLoginDTO> CollegaAccountSocialAsync(int utenteId, LoginOAuthDTO loginOAuthDto);

        // 🔹 Scollega un account social (Google/Apple) dall'utente
        Task<bool> ScollegaAccountSocialAsync(int utenteId, string provider);
    }
}
