namespace OrderAPI.RequestResponse;

public record OrderCreateRequest
{
    public OrderCreateRequest()
    {
        Items = new();
    }

    public string UserId { get; set; }
    public List<OrderItemRequest> Items { get; set; }
}

public record OrderItemRequest
{
    public int ProductId { get; set; }
    public int Count { get; set; }
    public int UnitPrice { get; set; }
}