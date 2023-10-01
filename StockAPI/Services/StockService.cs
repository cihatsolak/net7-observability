namespace StockAPI.Services;

public class StockService
{
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

        return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(StatusCodes.Status200OK, new StockCheckAndPaymentProcessResponseDto
        {
            Description = "Stock is reserved."
        });
    }
}
