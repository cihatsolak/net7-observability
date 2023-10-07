namespace Logging.Shared;

public class OpenTelemetryTraceIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<OpenTelemetryTraceIdMiddleware> _logger;

    public OpenTelemetryTraceIdMiddleware(
        RequestDelegate next, 
        ILogger<OpenTelemetryTraceIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString();

        using (_logger.BeginScope("{@traceId}", traceId))
        {
            await _next(context);
        }
    }
}