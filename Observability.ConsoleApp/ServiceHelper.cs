namespace Observability.ConsoleApp;

internal class ServiceHelper
{
    internal void Work1()
    {
        using var activity = ActivitySourceProvider.Source.StartActivity();

        int length = GoogleService.RequestToGoogleAsync().Result;

        Console.WriteLine($"google response length: {length}");


        Console.WriteLine("Work1 tamamlandı.");
    }
}
