using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Checkout.Queries;

namespace zizo_shop.API.Controllers.Orders
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var query = new GetMyOrdersQuery();
            var orders = await _mediator.Send(query, cancellationToken);
            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found for the current user.");
            }

            return Ok(orders);
        }
    }
}
