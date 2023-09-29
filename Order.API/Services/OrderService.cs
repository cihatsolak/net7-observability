namespace OrderAPI.Services;

public class OrderService
{
    public Task AddAsync(OrderCreateRequest request)
    {
        //Ana aktiviteye tag eklemek
        Activity.Current.SetTag("AspNetCore (Instrumentation) Tag 1", "AspNetCore (Instrumentation) Tag Value");

        using var activity = ActivitySourceProvider.Source.StartActivity();

        activity.AddEvent(new ActivityEvent("The order process has started."));

        //database operations

        activity.SetTag("Order User Id", request.UserId);

        activity.AddEvent(new ActivityEvent("The order process is completed."));

        return Task.CompletedTask;
    }
}
