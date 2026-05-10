using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Checkout.Commands;

namespace zizo_shop.API.Controllers.Checkout
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CheckoutController(IMediator mediator) { _mediator = mediator; }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutCommand command, CancellationToken cancellationToken)
        {
            var orderId = await _mediator.Send(command, cancellationToken);
            return Ok(new { OrderId = orderId, Message = "Order placed successfully." });
        }
    }
}
