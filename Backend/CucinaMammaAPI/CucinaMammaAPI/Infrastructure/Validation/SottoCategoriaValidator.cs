// ========== SottoCategoriaValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class SottoCategoriaValidator : AbstractValidator<SottoCategoriaDto>
{
    public SottoCategoriaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(50);
        RuleForEach(x => x.Immagini)
            .SetValidator(new ImmagineValidator());

        RuleForEach(x => x.CategorieSottoCategorie)
            .SetValidator(new CategoriaSottoCategoriaValidator());
    }
}
