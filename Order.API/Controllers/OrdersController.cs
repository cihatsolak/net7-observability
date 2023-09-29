namespace OrderAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult PreparedForErrorExample()
        {
            int number1 = 10;
            int number2 = 0;

            int result = number1 / number2; //divide by zero exception

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            await _orderService.AddAsync(request);
            return Ok();       
        }
    }
}
