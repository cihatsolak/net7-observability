var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryLog();
//builder.Host.UseSerilog(Logging.Shared.Logging.ConfigureLogging);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetryConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseExceptionMiddleware();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();