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
            configure
                .AddSource(openTelemetrySetting.ActivitySourceName)
                .AddSource(DiagnosticHeaders.DefaultListenerName) //Masstransit & rabbitmq
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

                // uygulama bir exception fırlattıgında buraya gelecek, istenirse ilgili exception'a ek özellikler eklenebilir.
                options.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("key1", "exception");
                };
            });

            configure.AddEntityFrameworkCoreInstrumentation(options =>
            {
                options.SetDbStatementForText = true; //db ifadelerini aktivite içerisine text olarak kaydedilim mi?
                options.SetDbStatementForStoredProcedure = true;

                //ekstra özellikler eklemek için
                //sql cümleciğimi aktivite olarak her kaydettiğinde bu metot tetiklenir. isternirse yanına ek bilgi eklenebilir.
                //ef core'un ürettiği aktiviteye tag'lar eventler eklenebilir.
                options.EnrichWithIDbCommand = (activity, dbCommand) =>
                {
                    activity.SetTag("key1", "exception");
                };
            });

            configure.AddHttpClientInstrumentation(options =>
            {
                options.EnrichWithHttpRequestMessage = async (activity, httpRequestMessage) =>
                {
                    string requestContent = "Empty Request!";

                    if (httpRequestMessage.Content is not null)
                    {
                        requestContent = await httpRequestMessage.Content.ReadAsStringAsync();
                    }

                    activity.SetTag("http.request.body", requestContent);
                };

                options.EnrichWithHttpResponseMessage = async (activity, httpResponseMessage) =>
                {
                    string responseContent = "Empty Response!";

                    if (httpResponseMessage.Content is not null)
                    {
                        responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    }

                    activity.SetTag("http.response.body", responseContent);
                };

                options.FilterHttpRequestMessage = (context) =>
                {
                    return !context.RequestUri.AbsoluteUri.Contains("9200", StringComparison.OrdinalIgnoreCase); //elastic search'e yapılan istekleri loglama
                };
            });

            configure.AddRedisInstrumentation(options =>
            {
                options.SetVerboseDatabaseStatements = true;
            });


            configure.AddConsoleExporter();
            configure.AddOtlpExporter(); //Jaeger
        });
    }
}