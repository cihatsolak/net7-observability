namespace MetricAPI.OpenTelemetry;

public static class OpenTelemetryMetric
{
    private static readonly Meter Meter = new Meter("metric.meter.api");

    public static readonly Counter<int> OrderCreatedEventCounter = Meter.CreateCounter<int>("order.created.event.count");
}
