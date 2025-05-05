// ========== RicettaValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class RicettaValidator : AbstractValidator<RicettaDTO>
{
    public RicettaValidator()
    {
        RuleFor(x => x.Titolo)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.TempoPreparazione)
            .GreaterThan(0);

        RuleFor(x => x.Difficolta)
            .NotEmpty()
            .Must(d => new[] { "Facile", "Media", "Difficile" }.Contains(d))
            .WithMessage("Difficoltà ammessa: Facile, Media, Difficile");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .Matches("^[a-z0-9-]+$");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(160);

        RuleForEach(x => x.Categorie)
            .SetValidator(new CategoriaValidator());

        RuleForEach(x => x.PassaggiPreparazione)
            .SetValidator(new PassaggioPreparazioneValidator());

        RuleForEach(x => x.Immagini)
            .SetValidator(new ImmagineValidator());

        RuleForEach(x => x.RicettaIngredienti)
            .SetValidator(new RicettaIngredienteValidator());
    }
}
