// ========== CommentoValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class CommentoValidator : AbstractValidator<CommentoDto>
{
    public CommentoValidator()
    {
        RuleFor(x => x.Testo)
            .NotEmpty().MaximumLength(1000);

        RuleFor(x => x.UtenteId).GreaterThan(0);
        RuleFor(x => x.RicettaId).GreaterThan(0);

        // DataCreazione opzionale: se presente non può essere futura
        When(x => x.DataCreazione != default, () =>
            RuleFor(x => x.DataCreazione)
               .LessThanOrEqualTo(DateTime.UtcNow));
    }
}
