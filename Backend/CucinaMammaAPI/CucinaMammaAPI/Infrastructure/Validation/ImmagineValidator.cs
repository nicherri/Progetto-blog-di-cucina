// ========== ImmagineValidator.cs ==========
using FluentValidation;
using CucinaMammaAPI.DTOs;

namespace CucinaMammaAPI.Infrastructure.Validation;

public sealed class ImmagineValidator : AbstractValidator<ImmagineDTO>
{
    public ImmagineValidator()
    {
        RuleFor(x => x.Alt)
            .NotEmpty().MaximumLength(120);

        RuleFor(x => x.Title)
            .MaximumLength(120);

        RuleFor(x => x.NomeFileSeo)
            .MaximumLength(120)
            .Matches("^[a-z0-9-]+$").When(x => !string.IsNullOrEmpty(x.NomeFileSeo));

        RuleFor(x => x.Ordine)
            .GreaterThanOrEqualTo(0);

        // Dimensione e tipo file (esempio 2 MB, png/jpg)
        When(x => x.File != null, () =>
        {
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(2 * 1024 * 1024).WithMessage("File max 2 MB");

            RuleFor(x => x.File!.ContentType)
                .Must(t => t is "image/png" or "image/jpeg")
                .WithMessage("Sono ammessi solo PNG o JPEG");
        });
    }
}
