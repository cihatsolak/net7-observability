// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

ActivitySource.AddActivityListener(new ActivityListener() //Custom olarak bir activity dinlemek istediğimde
{
    ShouldListenTo = source => source.Name == OpenTelemetryConstants.ActivitySourceFileName,
    ActivityStarted = activity =>
    {
        Console.WriteLine("Activity started.");
    },
    ActivityStopped = activity =>
    {
        Console.WriteLine("Activity stopped.");
    }
});

using var traceProviderFile = Sdk.CreateTracerProviderBuilder()
        .AddSource(OpenTelemetryConstants.ActivitySourceFileName)
        .Build();

using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(OpenTelemetryConstants.ActivitySourceName)
    .ConfigureResource(configure =>
    {
        configure.AddService(OpenTelemetryConstants.ServiceName, serviceVersion: OpenTelemetryConstants.ServiceVersion)
        .AddAttributes(new List<KeyValuePair<string, object>>()
        {
            new KeyValuePair<string, object>("host.machineName", Environment.MachineName),
            new KeyValuePair<string, object>("host.os", Environment.OSVersion.VersionString),
            new KeyValuePair<string, object>("dotnet.version", Environment.OSVersion.Version.ToString()),
            new KeyValuePair<string, object>("host.environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")),
        });
    })
    .AddConsoleExporter()
    .AddOtlpExporter() //jeager ui
    .AddZipkinExporter(zp =>
    {
        zp.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
    })
    .Build();  

var serviceHelper = new ServiceHelper();
serviceHelper.Work1();
serviceHelper.Work2();