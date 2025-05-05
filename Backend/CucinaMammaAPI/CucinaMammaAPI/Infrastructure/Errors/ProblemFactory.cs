using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CucinaMammaAPI.Infrastructure.Errors;

public static class ProblemFactory
{
    public static ApiProblemDetails Validation(
        IDictionary<string, string[]> errors,
        HttpContext ctx) =>
        new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validazione fallita",
            Type = "https://docs.cucinamamma.com/errors/validation",
            Instance = ctx.Request.Path,
            ErrorCode = "VAL-400-001",
            UserMessage = "Alcuni dati non sono corretti.",
            Errors = errors,
            TraceId = ctx.TraceIdentifier,
            Severity = ErrorSeverity.Warning
        };

    // puoi aggiungere altri factory method: NotFound, Unauthorized, ecc.
}
