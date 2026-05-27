using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Kaffebar.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("orders")]

    public class OrdersController : ControllerBase
    {
        private static readonly ConcurrentDictionary<Guid, CreateOrderResponse> Orders = new();

        [HttpPost]

        public IActionResult CreateOrder(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();
            var created = new CreateOrderResponse(orderId, request.CoffeeId, DateTime.UtcNow);
            Orders[orderId] = created;
            var location = $"/orders/{orderId}";
            return Created(location, created);
        }

        [HttpGet("{orderId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IResult GetOrder(Guid orderId)
        {
            if (Orders.TryGetValue(orderId, out var order))
            {
                return Results.Ok(order);
            } else {
                return Results.Problem("Order not found", statusCode: StatusCodes.Status404NotFound);
            }
        }
    }
}
