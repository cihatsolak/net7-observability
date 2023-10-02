namespace OrderAPI.Services;

public class StockService
{
    private readonly HttpClient _httpClient;

    public StockService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool succeeded, string errorMessage)> CheckAndPaymentStartAsync(StockCheckAndPaymentProcessRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync<StockCheckAndPaymentProcessRequestDto>("api/stocks", request);
        var responseContent = await response.Content.ReadFromJsonAsync<ResponseDto<StockCheckAndPaymentProcessResponseDto>>();

        if (!response.IsSuccessStatusCode)
        {
            return (default, responseContent.Errors[0]);
        }

        return (true, null);
    }
}
