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

    internal void Work2()
    {
        using var activity = ActivitySourceProvider.SourceFile.StartActivity();
        activity.SetTag("work 2 tag", "work 2 tag value");
        activity.AddEvent(new ActivityEvent("work 2 event"));
    }
}
