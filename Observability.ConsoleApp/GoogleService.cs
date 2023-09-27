namespace Observability.ConsoleApp;

internal class GoogleService
{
    static HttpClient httpClient = new HttpClient();

    internal async Task<int> RequestToGoogleAsync()
    {
        using var activity = ActivitySourceProvider.Source.StartActivity("GoogleActivity", ActivityKind.Server);
        var result = await httpClient.GetAsync("https://www.google.com");
        var responseContent = await result.Content.ReadAsStringAsync();

        return responseContent.Length;
    }
}
