// ========== LoginOAuthValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class LoginOAuthValidator : AbstractValidator<LoginOAuthDTO>
{
    public LoginOAuthValidator()
    {
        RuleFor(x => x.Provider).NotEmpty().Must(p => p is "Google" or "Apple");
        RuleFor(x => x.ProviderId).NotEmpty();
        // Email opzionale ma se presente deve essere valida
        When(x => !string.IsNullOrEmpty(x.Email), () =>
            RuleFor(x => x.Email!).EmailAddress());
    }
}
