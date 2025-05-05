// ========== RegisterOAuthValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class RegisterOAuthValidator : AbstractValidator<RegisterOAuthDTO>
{
    public RegisterOAuthValidator()
    {
        RuleFor(x => x.Provider).NotEmpty().Must(p => p is "Google" or "Apple");
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Cognome).NotEmpty();
    }
}
