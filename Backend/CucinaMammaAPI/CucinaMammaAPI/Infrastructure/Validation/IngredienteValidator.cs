// ========== IngredienteValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class IngredienteValidator : AbstractValidator<IngredienteDTO>
{
    public IngredienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().MaximumLength(100);

        RuleForEach(x => x.Immagini)
            .SetValidator(new ImmagineValidator());
    }
}
