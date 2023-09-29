namespace OpenTelemetry.Shared;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenTelemetrySetting>(configuration.GetSection("OpenTelemetry"));
        var openTelemetrySetting = configuration.GetSection("OpenTelemetry").Get<OpenTelemetrySetting>();

        ActivitySourceProvider.Source = new ActivitySource(openTelemetrySetting.ActivitySourceName);

        services.AddOpenTelemetry().WithTracing(configure =>
        {
            configure.AddSource(openTelemetrySetting.ActivitySourceName)
                .ConfigureResource(resource =>
                {
                    resource.AddService(openTelemetrySetting.ServiceName, serviceVersion: openTelemetrySetting.ServiceVersion);
                });

            configure.AddAspNetCoreInstrumentation(options =>
            {
                options.Filter = (context) =>
                {
                    // trace edilecekleri filtrele
                    // endpoint'de api geçenleri trace et.
                    return context.Request.Path.Value.Contains("api", StringComparison.OrdinalIgnoreCase);
                };

                //Exception oluşursa stack trace kaydedilir. Default olarak stack trace kaydedilmez.
                options.RecordException = true;
            });
            configure.AddConsoleExporter();
            configure.AddOtlpExporter(); //Jaeger
        });
    }
}