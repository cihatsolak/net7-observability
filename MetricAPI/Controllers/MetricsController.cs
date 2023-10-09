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
}
