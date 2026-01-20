using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Cart.Commands;
using zizo_shop.Application.Features.Cart.Queries;

namespace zizo_shop.API.Controllers.Cart
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMyCart), null, new { message = "Added Successfully" });
        }
        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            return Ok(await _mediator.Send(new GetCartQuery()));
        }
        [HttpGet]
        [Route("item-count")]
        public async Task<IActionResult> GetCartItemCount()
        {
            return Ok(await _mediator.Send(new GetCartItemCountQuery()));
        }
        [HttpGet]
        [Route("total-price")]
        public async Task<IActionResult> GetCartTotalPrice()
        {
            return Ok(await _mediator.Send(new GetCartTotalPriceQuery()));
        }
        [HttpGet]
        [Route("is-in-cart/{productId}")]
        public async Task<IActionResult> IsInCart(Guid productId)
        {
            return Ok(await _mediator.Send(new IsInCartQuery { ProductId = productId }));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemQuantityCommand command)
        {
            await _mediator.Send(command);
            return Ok("Updated Successfully");
        }
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            await _mediator.Send(new RemoveFromCartCommand (productId));
            return Ok("Removed Successfully");
        }
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _mediator.Send(new ClearCartCommand());
            return Ok("Cart Cleared Successfully");
        }

    }
}
