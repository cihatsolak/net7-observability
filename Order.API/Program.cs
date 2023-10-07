using Logging.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Logging.Shared.Logging.ConfigureLogging);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(AppDbContext)));
});

builder.Services.AddScoped<OrderService>();

builder.Services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString(nameof(RedisService))));
builder.Services.AddSingleton<RedisService>();

builder.Services.AddHttpClient<StockService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration["StockService"]);
});

builder.Services.AddOpenTelemetryConfiguration(builder.Configuration); //OpenTelemetry.Shared

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Username("guest");
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseMiddleware<OpenTelemetryTraceIdMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();