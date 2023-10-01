namespace StockAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly StockService _stockService;

    public StocksController(StockService stockService)
    {
        _stockService = stockService;
    }

    [HttpPost]
    public IActionResult CheckAndPaymentStart(StockCheckAndPaymentProcessRequestDto request)
    {
        var result = _stockService.CheckAndPaymentProcess(request);

        return new ObjectResult(result) { StatusCode = result.StatusCode };
    }
}
