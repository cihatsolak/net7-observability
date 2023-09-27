namespace Observability.ConsoleApp;

internal class ServiceHelper
{
    internal void Work1()
    {
        using var activity = ActivitySourceProvider.Source.StartActivity();

        var googleService = new GoogleService();
        int length = googleService.RequestToGoogleAsync().Result;

        Console.WriteLine($"google response length: {length}");


        Console.WriteLine("Work1 tamamlandı.");
    }
}
