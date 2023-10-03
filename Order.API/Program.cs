using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<RequestAndResponseActivityMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();