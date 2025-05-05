// ========== FatteDaVoiValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class FatteDaVoiValidator : AbstractValidator<FatteDaVoiDto>
{
    public FatteDaVoiValidator()
    {
        RuleFor(x => x.UtenteId).GreaterThan(0);
        RuleFor(x => x.RicettaId).GreaterThan(0);

        RuleFor(x => x.DataCaricamento)
            .LessThanOrEqualTo(DateTime.UtcNow);

        RuleForEach(x => x.Immagini)
            .SetValidator(new ImmagineValidator());

        // File immagine opzionale: max 3 MB, jpg/png
        When(x => x.ImmagineFile != null, () =>
        {
            RuleFor(x => x.ImmagineFile!.Length)
                .LessThanOrEqualTo(3 * 1024 * 1024).WithMessage("File max 3 MB");

            RuleFor(x => x.ImmagineFile!.ContentType)
                .Must(t => t is "image/png" or "image/jpeg")
                .WithMessage("Sono ammessi solo PNG o JPEG");
        });
    }
}
