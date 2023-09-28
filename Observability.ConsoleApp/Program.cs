// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

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
    .Build();  

var serviceHelper = new ServiceHelper();
serviceHelper.Work1();