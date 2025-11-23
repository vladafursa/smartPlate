using Microsoft.AspNetCore.Mvc;
using SmartPlate.Services.OrderService;
using SmartPlate.DTOs.Order;
using System.Security.Claims;

namespace SmartPlate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
            var buyerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _service.CreateOrderAsync(dto, buyerId);
            return Ok(order);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] OrderFilterDto filter)
        {
            var orders = await _service.FilterOrdersAsync(filter);
            return Ok(orders);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var order = await _service.CompleteOrderAsync(id);
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var order = await _service.CancelOrderAsync(id);
            return Ok(order);
        }
    }
}