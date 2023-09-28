var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenTelemetryConstants>(builder.Configuration.GetSection("OpenTelemetry"));

var openTelemetryConstants = builder.Configuration.GetSection("OpenTelemetry").Get<OpenTelemetryConstants>();

builder.Services.AddOpenTelemetry().WithTracing(configure =>
{
    configure.AddSource(openTelemetryConstants.ActivitySourceName)
        .ConfigureResource(resource =>
        {
            resource.AddService(openTelemetryConstants.ServiceName, serviceVersion: openTelemetryConstants.ServiceVersion);
        });

    configure.AddAspNetCoreInstrumentation();
    configure.AddConsoleExporter();
    configure.AddOtlpExporter(); //Jaeger
});

ActivitySourceProvider.Source = new ActivitySource(openTelemetryConstants.ActivitySourceName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();