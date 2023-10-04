namespace Common.Shared.Events;

public record OrderCreatedEvent
{
    public string OrderCode { get; set; }

    public OrderCreatedEvent()
    {
    }

    public OrderCreatedEvent(string orderCode)
    {
        OrderCode = orderCode;
    }
}
