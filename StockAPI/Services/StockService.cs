namespace StockAPI.Services;

public class StockService
{
    private readonly PaymentService _paymentService;
    private readonly ILogger<StockService> _logger;

    public StockService(PaymentService paymentService, ILogger<StockService> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public static Dictionary<int, int> GetStocks()
    {
        Dictionary<int, int> stocks = new()
        {
            { 1, 10 },
            { 2, 20 },
            { 3, 30 }
        };

        return stocks;
    }

    public ResponseDto<StockCheckAndPaymentProcessResponseDto> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto request)
    {
        //HttpContext.Request.Headers["Correlation-Context"];
        string userId = Activity.Current.GetBaggageItem("userId");

        //Stock Check and payment process request
        var productStockList = GetStocks();
        var stockStatus = new List<(int productId, bool hasStockExist)>();


        foreach (var orderItem in request.OrderItems)
        {
            var hasExistStock = productStockList.Any(p => p.Key == orderItem.ProductId && p.Value >= orderItem.Count);
            stockStatus.Add((orderItem.ProductId, hasExistStock));
        }

        if (stockStatus.Any(x => !x.hasStockExist))
        {
            return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Fail(StatusCodes.Status400BadRequest, "insufficient stock");
        }

        _logger.LogInformation("stock reserved. Order Code: {@orderCode}", request.OrderCode);

        var (succeeded, errorMessage) = _paymentService.CreatePaymentProcessAsync(new PaymentCreateRequestDto
        {
            OrderCode = request.OrderCode,
            TotalPrice = request.OrderItems.Sum(p => (p.UnitPrice * p.Count))
        }).Result;

        if (!succeeded)
        {
            return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(StatusCodes.Status200OK, new StockCheckAndPaymentProcessResponseDto
            {
                Description = errorMessage
            });
        }

        return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(StatusCodes.Status200OK, new StockCheckAndPaymentProcessResponseDto
        {
            Description = "payment process completed."
        });
    }
}
