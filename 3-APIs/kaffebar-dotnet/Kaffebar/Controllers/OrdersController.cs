using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kaffebar.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();
            var created = new CreateOrderResponse(orderId, request.CoffeeId, DateTime.UtcNow);
            var location = $"/orders/{orderId}";
            return Created(location, created);
        }
    }
}
