namespace MetricAPI.OpenTelemetry
{
    /// <summary>
    /// CounterObservable metric
    /// </summary>
    public static class Counters
    {
        public static int OrderCancelledCounter { get; set; }
        public static int CurrentCountStock { get; set; }
        public static int KitchenTemperature { get; set; }
    }
}
