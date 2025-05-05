// ========== UtenteValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class UtenteValidator : AbstractValidator<UtenteDTO>
{
    public UtenteValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cognome).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
