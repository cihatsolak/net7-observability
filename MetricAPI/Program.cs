var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry().WithMetrics(opt =>
{
    opt.AddMeter("metric.meter.api");

    opt.ConfigureResource(resource =>
    {
        resource.AddService("Metric.API", serviceVersion: "1.0.0");
    });

    opt.AddPrometheusExporter();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();

app.UseOpenTelemetryPrometheusScrapingEndpoint(); //metrics isminde endpoint oluþturacak.

app.MapControllers();

app.Run();
