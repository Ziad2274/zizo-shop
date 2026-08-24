using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Checkout.Queries;
using zizo_shop.Application.Features.Orders.Commands;
using zizo_shop.Application.Features.Orders.Queries;

namespace zizo_shop.API.Controllers.Orders
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator) => _mediator = mediator;

        // User: get own orders
        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders(CancellationToken ct)
            => Ok(await _mediator.Send(new GetMyOrdersQuery(), ct));

        [HttpGet("my/{id:guid}")]
        public async Task<IActionResult> GetMyOrderDetail(Guid id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetMyOrderDetailQuery(id), ct)); 
        // Admin: get all orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _mediator.Send(new GetAllOrdersQuery(), ct));

        // Admin: get order by id
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
            => Ok(await _mediator.Send(new GetOrderByIdQuery(id), ct));

        // Admin: update order status
        [HttpPatch("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusCommand cmd, CancellationToken ct)
        {
            await _mediator.Send(cmd with { OrderId = id }, ct);
            return Ok("Order status updated.");
        }

 

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new CancelMyOrderCommand(id), ct);
            return Ok("Order cancelled.");
        }
        

    }
}
