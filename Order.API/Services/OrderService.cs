namespace OrderAPI.Services;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public Task<OrderCreateResponse> AddAsync(OrderCreateRequest request)
    {
        //Ana aktiviteye tag eklemek
        Activity.Current.SetTag("AspNetCore (Instrumentation) Tag 1", "AspNetCore (Instrumentation) Tag Value");

        using var activity = ActivitySourceProvider.Source.StartActivity();

        activity.AddEvent(new ActivityEvent("The order process has started."));

        //database operations

        var order = new Order
        {
            CreatedAt = DateTime.Now,
            Code = Guid.NewGuid().ToString(),
            Status = OrderStatus.Success,
            UserId = request.UserId,
            Items = request.Items.Select(requestItem => new OrderItem
            {
                Count = requestItem.Count,
                Price = requestItem.UnitPrice,
                ProductId = requestItem.ProductId
            }).ToList()
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        activity.SetTag("Order User Id", request.UserId);

        activity.AddEvent(new ActivityEvent("The order process is completed."));

        return Task.FromResult(new OrderCreateResponse { Id = order.Id });
    }
}
