// ========== RicettaIngredienteValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class RicettaIngredienteValidator : AbstractValidator<RicettaIngredienteDTO>
{
    public RicettaIngredienteValidator()
    {
        RuleFor(x => x.IngredienteId).GreaterThan(0);
        RuleFor(x => x.Quantita).GreaterThan(0);
        RuleFor(x => x.UnitaMisura).NotEmpty();
    }
}
