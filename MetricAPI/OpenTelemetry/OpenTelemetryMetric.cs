namespace MetricAPI.OpenTelemetry;

public static class OpenTelemetryMetric
{
    private static readonly Meter Meter = new("metric.meter.api");

    //COUNTER
    public static readonly Counter<int> OrderCreatedEventCounter = Meter.CreateCounter<int>("order.created.event.count");

    //COUNTER OBSERVABLE
    public static readonly ObservableCounter<int> OrderCancelledCounter
            = Meter.CreateObservableCounter("order.cancel.count", () =>
            {
                return new Measurement<int>(Counters.OrderCancelledCounter);
            });

    //UPDOWN COUNTER
    public static UpDownCounter<int> CurrentStockCounter = Meter.CreateUpDownCounter<int>("current.stock.count", unit: "birim", "lorem ipsum.");

    //UPDOWN COUNTER OBSERVABLE
    public static ObservableUpDownCounter<int> CurrentStockObservableCounter
        = Meter.CreateObservableUpDownCounter("current.stock.observable.counter", () =>
        {
            return new Measurement<int>(Counters.CurrentCountStock);
        });

    //GAUGE OBSERVABLE
    public static ObservableGauge<int> RowKitchenTemp
        = Meter.CreateObservableGauge<int>("row.kitchen.temperature", () =>
        {
            return new Measurement<int>(Counters.KitchenTemperature);
        });
}
