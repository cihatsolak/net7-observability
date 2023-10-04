namespace StockAPI.Consumers;

public sealed class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    public Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        Thread.Sleep(TimeSpan.FromSeconds(2));

        Activity.Current.SetTag("message.body", JsonSerializer.Serialize(context.Message));

        return Task.CompletedTask;
    }
}
