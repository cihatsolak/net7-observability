namespace Observability.ConsoleApp;

internal static class GoogleService
{
    static readonly HttpClient httpClient = new HttpClient();

    internal static async Task<int> RequestToGoogleAsync()
    {
        using var activity = ActivitySourceProvider.Source.StartActivity("GoogleActivity", ActivityKind.Server);

        try
        {
            ActivityTagsCollection eventTags = new() //aktivite ile ilgili ekstra bilgiler kaydetmek istiyorsak
            {
                { "userId", 30 }
            };

            activity.AddEvent(new ActivityEvent("The request to Google started.", tags: eventTags));

            var result = await httpClient.GetAsync("https://www.google.com");

            activity.AddTag("request.schema", "https");
            activity.AddTag("request.method", "GET");

            var responseContent = await result.Content.ReadAsStringAsync();
             

            eventTags.Add("Google Body Lenght", responseContent.Length); //aktivite ile ilgili ekstra bilgiler kaydetmek istiyorsak

            activity.AddEvent(new ActivityEvent("Google request completed.", tags: eventTags));

            int fileResponseLenght = FileService.WriteToFileAsync("hello!").Result;


            return responseContent.Length;
        }
        catch (Exception ex)
        {
            activity.SetStatus(ActivityStatusCode.Error, ex.Message);
            throw;
        }
    }
}
