using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CucinaMammaAPI.Infrastructure.Errors

{
    /// <summary>
    /// Formato d’errore “enterprise” aderente a RFC 7807 con estensioni.
    /// </summary>
    public sealed class ApiProblemDetails : ProblemDetails
    {
        /* ===== RFC 7807 standard fields (inherited) =====
           - Status (int)
           - Title  (string)
           - Detail (string)
           - Type   (string - URI)
           - Instance (string)
        */

        /* ===== Estensioni “enterprise” ===== */

        /// <summary> GUID che identifica in modo univoco l’errore nell’infrastruttura. </summary>
        [JsonPropertyName("errorId")]
        public Guid ErrorId { get; init; } = Guid.NewGuid();

        /// <summary> Trace-Id o Correlation-Id (uguale a quello propagato nei log/OpenTelemetry). </summary>
        [JsonPropertyName("traceId")]
        public string? TraceId { get; init; }

        /// <summary> Codice interno di dominio (es. IMG-VAL-001, AUTH-401-TOKEN). </summary>
        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; init; }

        /// <summary> Severità logica dell’errore. </summary>
        [JsonPropertyName("severity")]
        public ErrorSeverity Severity { get; init; } = ErrorSeverity.Error;

        /// <summary> Messaggio pensato per l’utente finale, localizzabile. </summary>
        [JsonPropertyName("userMessage")]
        public string? UserMessage { get; init; }

        /// <summary> Unix epoch ms in cui l’errore è avvenuto (utile per ordering). </summary>
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// URL alla documentazione specifica (how-to, FAQ, pagina supporto).
        /// </summary>
        [JsonPropertyName("documentationUri")]
        public string? DocumentationUri { get; init; }

        /// <summary>
        /// Numero di secondi consigliato prima di riprovare (per rate-limit o lock).
        /// </summary>
        [JsonPropertyName("retryAfter")]
        public int? RetryAfter { get; init; }

        /// <summary>
        /// Dettagli di validazione o errori campo-specifici.
        /// key = nomeCampo, value = elenco messaggi.
        /// </summary>
        [JsonPropertyName("errors")]
        public IDictionary<string, string[]>? Errors { get; init; }

        /// <summary>
        /// Errori “figli” (per propagare inner-exception o aggregazioni).
        /// </summary>
        [JsonPropertyName("innerErrors")]
        public IList<ApiProblemDetails>? InnerErrors { get; init; }
    }

    /// <summary>Gravità logica usata da UI / logging.</summary>
    public enum ErrorSeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Fatal = 3
    }
}