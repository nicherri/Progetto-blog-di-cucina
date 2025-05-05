using CucinaMammaAPI.Infrastructure.Errors;
using FluentValidation;          // <-- unico namespace con ValidationException
using System.Text.Json;

namespace CucinaMammaAPI.Infrastructure.Middleware;
public sealed class GlobalErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalErrorMiddleware(RequestDelegate next,
                                 ILogger<GlobalErrorMiddleware> logger,
                                 IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            var traceId = ctx.TraceIdentifier;

            var status = ex switch
            {
                FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            _logger.LogError(ex, "[{TraceId}] Unhandled exception", traceId);

            var problem = new ApiProblemDetails
            {
                Status = status,
                Title = status switch
                {
                    400 => "Richiesta non valida",
                    401 => "Non autorizzato",
                    404 => "Risorsa non trovata",
                    _ => "Errore del server"
                },
                Detail = _env.IsDevelopment() ? ex.ToString() : null,
                Type = $"https://docs.lac.com/errors/{status}",
                Instance = ctx.Request.Path,
                TraceId = traceId,
                Severity = status >= 500 ? ErrorSeverity.Fatal : ErrorSeverity.Error,
                UserMessage = status switch
                {
                    400 => "Alcuni dati non sono corretti.",
                    401 => "Effettua di nuovo il login.",
                    404 => "La risorsa richiesta non esiste.",
                    _ => "Si è verificato un errore inatteso. Riprova più tardi."
                },
                ErrorCode = MapToErrorCode(ex),
                Errors = ex is FluentValidation.ValidationException ve ? MapValidation(ve) : null,
                DocumentationUri = $"https://docs.lac.com/errors/{MapToErrorCode(ex)}"
            };

            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";

            var opts = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem, opts));
        }
    }

    private static string MapToErrorCode(Exception ex) =>
        ex switch
        {
            FluentValidation.ValidationException => "VAL-001",
            UnauthorizedAccessException => "AUTH-401-001",
            KeyNotFoundException => "GEN-404-001",
            _ => "GEN-500-001"
        };

    private static IDictionary<string, string[]> MapValidation(FluentValidation.ValidationException e) =>
        e.Errors
         .GroupBy(v => v.PropertyName)
         .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage).ToArray());
}
