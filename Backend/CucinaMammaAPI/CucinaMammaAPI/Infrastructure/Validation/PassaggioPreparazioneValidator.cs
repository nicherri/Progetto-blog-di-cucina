// ========== PassaggioPreparazioneValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class PassaggioPreparazioneValidator : AbstractValidator<PassaggioPreparazioneDTO>
{
    public PassaggioPreparazioneValidator()
    {
        RuleFor(x => x.Ordine).GreaterThan(0);
        RuleFor(x => x.Descrizione).NotEmpty();
        When(x => x.Immagine != null, () =>
        {
            RuleFor(x => x.Immagine!).SetValidator(new ImmagineValidator());
        });
    }
}
