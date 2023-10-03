namespace PaymentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PaymentsController : ControllerBase
{
    [HttpPost("create-process")]
    public IActionResult Create(PaymentCreateRequestDto request)
    {
        const decimal balance = 1000;

        if (request.TotalPrice > balance)
        {
            return BadRequest(ResponseDto<PaymentCreateResponseDto>.Fail(StatusCodes.Status200OK, "insufficient funds."));
        }

        return Ok(ResponseDto<PaymentCreateResponseDto>
            .Success(StatusCodes.Status200OK, new PaymentCreateResponseDto { Description = "the payment transaction was completed successfully." }));
    }
}
