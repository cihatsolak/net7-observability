var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<StockService>();

builder.Services.AddOpenTelemetryConfiguration(builder.Configuration); //OpenTelemetry.Shared

builder.Services.AddHttpClient<PaymentService>(options =>
{
    options.BaseAddress = new Uri(builder.Configuration[nameof(PaymentService)]);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Username("guest");
        });

        cfg.ReceiveEndpoint("stock.order-created-event.queue", endpoint =>
        {
            endpoint.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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