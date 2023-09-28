namespace Observability.ConsoleApp;

internal static class FileService
{
    internal static async Task<int> WriteToFileAsync(string text)
    {
        using var activity = ActivitySourceProvider.Source.StartActivity();

        await File.WriteAllTextAsync("myFile.txt", text);

        return (await File.ReadAllBytesAsync("myFile.txt")).Length;
    }
}
