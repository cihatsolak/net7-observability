﻿using Order = OrderAPI.Models.Order;

namespace OrderAPI.Services;

public class OrderService
{
    private readonly AppDbContext _context;
    private readonly StockService _stockService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        AppDbContext appDbContext,
        StockService stockService,
        ILogger<OrderService> logger)
    {
        _context = appDbContext;
        _stockService = stockService;
        _logger = logger;
    }

    public async Task<ResponseDto<OrderCreateResponse>> AddAsync(OrderCreateRequest request)
    {
        //Ana aktiviteye tag eklemek
        Activity.Current.SetTag("AspNetCore (Instrumentation) Tag 1", "AspNetCore (Instrumentation) Tag Value");

        using var activity = ActivitySourceProvider.Source.StartActivity();

        activity.AddEvent(new ActivityEvent("The order process has started."));

        //farklı bir api'den bu değere erişebilmek için
        activity.SetBaggage("userId", request.UserId.ToString());

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

        _logger.LogInformation("Order completed.  user id: {@userId}", request.UserId);

        var stockResponse = await _stockService.CheckAndPaymentStartAsync(new StockCheckAndPaymentProcessRequestDto
        {
            OrderCode = order.Code,
            OrderItems = request.Items
        });

        if (!stockResponse.succeeded)
        {
            return ResponseDto<OrderCreateResponse>.Fail(StatusCodes.Status500InternalServerError, stockResponse.errorMessage);
        }

        activity.SetTag("Order User Id", request.UserId);

        activity.AddEvent(new ActivityEvent("The order process is completed."));

        return ResponseDto<OrderCreateResponse>.Success(StatusCodes.Status200OK, new OrderCreateResponse { Id = order.Id });
    }
}
