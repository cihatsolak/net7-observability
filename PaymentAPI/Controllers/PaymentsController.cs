namespace PaymentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("create-process")]
    public IActionResult Create(PaymentCreateRequestDto request)
    {
        const decimal balance = 1000;

        if (request.TotalPrice > balance)
        {
            _logger.LogWarning("insufficient funds. Order Code: {@orderCode}", request.OrderCode);

            return BadRequest(ResponseDto<PaymentCreateResponseDto>.Fail(StatusCodes.Status200OK, "insufficient funds."));
        }

        _logger.LogInformation("The card transaction was completed successfully. Order Code: {@orderCode}", request.OrderCode);

        return Ok(ResponseDto<PaymentCreateResponseDto>
            .Success(StatusCodes.Status200OK, new PaymentCreateResponseDto { Description = "the payment transaction was completed successfully." }));
    }
}
