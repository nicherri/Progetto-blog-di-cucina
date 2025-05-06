using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CucinaMammaAPI.Infrastructure.Errors;

/// <summary>
/// Factory centralizzata per oggetti <see cref="ApiProblemDetails"/>.
/// Garantisce coerenza tra titolo, errorCode, link alla documentazione e metadati.
/// </summary>
public static class ProblemFactory
{
    /* ══════════════════════════════════ PUBLIC API ═════════════════════════════════ */

    /// <summary>400 – Errore di validazione formale/semantica.</summary>
    public static ApiProblemDetails Validation(ModelStateDictionary modelState, HttpContext ctx)
        => Validation(ToErrorDictionary(modelState), ctx);

    /// <summary>400 – Errore di validazione con dizionario già pronto.</summary>
    public static ApiProblemDetails Validation(IDictionary<string, string[]> errors, HttpContext ctx)
        => Build(StatusCodes.Status400BadRequest, ctx,
                 title: "Validazione fallita",
                 errorCode: "VAL-400-001",
                 userMessage: "Alcuni dati non sono corretti.",
                 severity: ErrorSeverity.Warning,
                 errors: errors);

    /// <summary>401 – Token mancante o non valido.</summary>
    public static ApiProblemDetails Unauthorized(HttpContext ctx)
        => Build(StatusCodes.Status401Unauthorized, ctx,
                 title: "Non autenticato",
                 errorCode: "AUTH-401-001",
                 userMessage: "Effettua di nuovo l’accesso.");

    /// <summary>403 – Utente autenticato ma privo di permessi.</summary>
    public static ApiProblemDetails Forbidden(HttpContext ctx)
        => Build(StatusCodes.Status403Forbidden, ctx,
                 title: "Accesso negato",
                 errorCode: "AUTH-403-001",
                 userMessage: "Non hai i permessi per questa operazione.");

    /// <summary>404 – Risorsa non esistente.</summary>
    public static ApiProblemDetails NotFound(string resourceName, HttpContext ctx)
        => Build(StatusCodes.Status404NotFound, ctx,
                 title: "Risorsa non trovata",
                 errorCode: "GEN-404-001",
                 userMessage: $"{resourceName} non esiste o è stata rimossa.");

    /// <summary>409 – Conflitto di stato (es. slug duplicato).</summary>
    public static ApiProblemDetails Conflict(string detail, HttpContext ctx)
        => Build(StatusCodes.Status409Conflict, ctx,
                 title: "Conflitto",
                 errorCode: "GEN-409-001",
                 userMessage: detail,
                 detail: detail);

    /// <summary>429 – Rate-limit superato.</summary>
    public static ApiProblemDetails TooManyRequests(int retryAfterSec, HttpContext ctx)
        => Build(StatusCodes.Status429TooManyRequests, ctx,
                 title: "Troppe richieste",
                 errorCode: "RATE-429-001",
                 userMessage: "Hai effettuato troppe richieste. Riprova a breve.",
                 retryAfter: retryAfterSec);

    /// <summary>500 – Errore interno inatteso.</summary>
    public static ApiProblemDetails Internal(string? detail, HttpContext ctx, string code = "GEN-500-001")
        => Build(StatusCodes.Status500InternalServerError, ctx,
                 title: "Errore del server",
                 errorCode: code,
                 userMessage: "Si è verificato un errore inatteso. Riprovare più tardi.",
                 severity: ErrorSeverity.Fatal,
                 detail: detail);

    /* ════════════════════════════════ EXTENSION HELPERS ═════════════════════════════ */

    /// <summary>
    /// Converte l’oggetto in <see cref="ObjectResult"/> pronto per <c>return</c> in controller.
    /// </summary>
    public static ObjectResult ToObjectResult(this ApiProblemDetails problem)
        => new(problem)
        {
            StatusCode = problem.Status,
            ContentTypes = { "application/problem+json" }
        };

    /* ══════════════════════════════════ INTERNALS ═══════════════════════════════════ */

    private static ApiProblemDetails Build(
        int status, HttpContext ctx,
        string title,
        string errorCode,
        string userMessage,
        ErrorSeverity severity = ErrorSeverity.Error,
        string? detail = null,
        int? retryAfter = null,
        IDictionary<string, string[]>? errors = null)
    {
        return new ApiProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = $"https://docs.cucinamamma.com/errors/{errorCode}",
            Instance = ctx.Request.Path,
            ErrorCode = errorCode,
            UserMessage = userMessage,
            Severity = severity,
            RetryAfter = retryAfter,
            Errors = errors,
            TraceId = ctx.TraceIdentifier
        };
    }

    private static IDictionary<string, string[]> ToErrorDictionary(ModelStateDictionary ms)
        => ms.ToDictionary(k => k.Key, v => v.Value!.Errors.Select(e => e.ErrorMessage)
                                                           .DefaultIfEmpty("Dato non valido")
                                                           .ToArray());
}
