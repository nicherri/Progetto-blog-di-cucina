// ========== CategoriaSottoCategoriaValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class CategoriaSottoCategoriaValidator : AbstractValidator<CategoriaSottoCategoriaDto>
{
    public CategoriaSottoCategoriaValidator()
    {
        RuleFor(x => x.CategoriaId).GreaterThan(0);
        RuleFor(x => x.SottoCategoriaId).GreaterThan(0);
    }
}
