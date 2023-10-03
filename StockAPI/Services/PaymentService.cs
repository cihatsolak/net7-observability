namespace StockAPI.Services;

public class PaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool succeeded, string errorMessage)> CreatePaymentProcessAsync(PaymentCreateRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/payments/create-process", request);
        var responseContent = await response.Content.ReadFromJsonAsync<ResponseDto<PaymentCreateResponseDto>>();

        if (!response.IsSuccessStatusCode)
        {
            return (default, responseContent.Errors[0]);
        }

        return (true, null);
    }
}
