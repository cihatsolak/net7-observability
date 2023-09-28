namespace Observability.ConsoleApp;

internal class ServiceHelper
{
    internal void Work1()
    {
        using var activity = ActivitySourceProvider.Source.StartActivity();

        int googleResponseLength = GoogleService.RequestToGoogleAsync().Result;
               
        Console.WriteLine($"google response length: {googleResponseLength}");
        Console.WriteLine("Work1 tamamlandı.");
    }
}
