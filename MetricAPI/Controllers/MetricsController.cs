namespace MetricAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MetricsController : ControllerBase
{
    [HttpGet]
    public IActionResult CounterMetric()
    {
        //Kuyruğa mesaj gönderildi.
        //diğer işlemler vs.

        OpenTelemetryMetric.OrderCreatedEventCounter.Add(1, //kuyruğa gönderdiğim mesaj adedini ekliyorum.
            new KeyValuePair<string, object>("event", "add"),
            new KeyValuePair<string, object>("queue.name", "event.created.queue")
            ); 

        return Ok();
    }

    [HttpGet]
    public IActionResult ObservableCounterMetric()
    {
        Counters.OrderCancelledCounter = new Random().Next(1, 100);

        return Ok();
    }

    [HttpGet]
    public IActionResult UpDowmnCounterMetric()
    {
        OpenTelemetryMetric.CurrentStockCounter.Add(new Random().Next(-300, 300)); // - (eksi) değer verebilirim.

        return Ok();
    }

    [HttpGet]
    public IActionResult ObservableUpDowmnCounterMetric()
    {
        Counters.CurrentCountStock += new Random().Next(-300, 300); // - (eksi) değer verebilirim.

        return Ok();
    }

    [HttpGet]
    public IActionResult ObservableGaugeMetric()
    {
        Counters.KitchenTemperature += new Random().Next(-30, 60);

        return Ok();
    }

    [HttpGet]
    public IActionResult HistogramMetric()
    {
        OpenTelemetryMetric.MethodDuration.Record(new Random().Next(500, 50000));
       
        return Ok();
    }
}
