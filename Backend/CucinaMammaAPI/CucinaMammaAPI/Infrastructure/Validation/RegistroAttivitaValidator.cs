// ========== RegistroAttivitaValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class RegistroAttivitaValidator : AbstractValidator<RegistroAttivitaDTO>
{
    public RegistroAttivitaValidator()
    {
        RuleFor(x => x.UtenteId).GreaterThan(0);
        RuleFor(x => x.NomeUtente).NotEmpty();

        RuleFor(x => x.Azione).IsInEnum();

        RuleFor(x => x.Data)
            .LessThanOrEqualTo(DateTime.UtcNow);

        // Ip e device stringhe opzionali ma max 100 char
        RuleFor(x => x.IndirizzoIP).MaximumLength(100);
        RuleFor(x => x.Nazione).MaximumLength(100);
        RuleFor(x => x.Citta).MaximumLength(100);
        RuleFor(x => x.Dispositivo).MaximumLength(100);
        RuleFor(x => x.Browser).MaximumLength(100);

        // Se RicettaId presente, deve essere >0 e avere NomeRicetta
        When(x => x.RicettaId.HasValue, () =>
        {
            RuleFor(x => x.RicettaId!.Value).GreaterThan(0);
            RuleFor(x => x.NomeRicetta).NotEmpty();
        });
    }
}
