namespace OrderAPI.RequestResponse;

public record OrderCreateRequest
{
    public OrderCreateRequest()
    {
        Items = new();
    }

    public int UserId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
