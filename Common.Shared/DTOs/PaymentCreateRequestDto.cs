namespace Common.Shared.DTOs;

public record class PaymentCreateRequestDto
{
    public string OrderCode { get; set; }
    public decimal TotalPrice { get; set; }
}
