// ========== CategoriaValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class CategoriaValidator : AbstractValidator<CategoriaDTO>
{
    public CategoriaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .Matches("^[a-z0-9-]+$").WithMessage("Solo lower-case, numeri, trattini.");

        RuleFor(x => x.SeoTitle)
            .MaximumLength(70);

        RuleFor(x => x.SeoDescription)
            .MaximumLength(160);

        RuleForEach(x => x.Immagini)
            .SetValidator(new ImmagineValidator());

        RuleForEach(x => x.CategorieSottoCategorie)
            .SetValidator(new CategoriaSottoCategoriaValidator());
    }
}
