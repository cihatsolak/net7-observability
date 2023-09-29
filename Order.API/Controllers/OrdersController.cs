namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            int number1 = 10;
            int number2 = 0;

            int result = number1 / number2; //divide by zero exception

            return Ok(result);
        }
    }
}
