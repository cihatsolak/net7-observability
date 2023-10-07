﻿namespace OrderAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrdersController : ControllerBase
{
    readonly OrderService _orderService;
    readonly RedisService _redisService;
    readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(
        OrderService orderService,
        RedisService redisService,
        IPublishEndpoint publishEndpoint)
    {
        _orderService = orderService;
        _redisService = redisService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public IActionResult PreparedForErrorExample()
    {
        int number1 = 10;
        int number2 = 0;

        int result = number1 / number2; //divide by zero exception

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateRequest request)
    {
        var response = await _orderService.AddAsync(request);

        return new ObjectResult(response) { StatusCode = response.StatusCode };
    }

    [HttpGet("redis-test")]
    public IActionResult RedisTest()
    {
        // Nuget paketi redis için otomatik activity oluşturduğu için gerek yok
        // using var redisActivity = ActivitySourceProvider.Source.StartActivity("Redis.Source"); 

        _redisService.GetDatabase(0).StringSet("user-id", 1);

        string userId = _redisService.GetDatabase(5).StringGet("user-id");

        return Ok(userId);
    }

    [HttpGet("rabbitmq-test")]
    public async Task<IActionResult> RabbitMqTest()
    {
        await _publishEndpoint.Publish(new OrderCreatedEvent(Guid.NewGuid().ToString())); //rabbitmq tracking

        return Ok();
    }
}
