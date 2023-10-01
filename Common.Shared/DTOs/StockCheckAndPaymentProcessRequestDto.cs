namespace Common.Shared.DTOs;

public record StockCheckAndPaymentProcessRequestDto
{
    public string OrderCode { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}
