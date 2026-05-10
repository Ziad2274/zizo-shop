using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Wishlist.Commands;
using zizo_shop.Application.Features.Wishlist.Queries;

namespace zizo_shop.API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWishlist()
            => Ok(await _mediator.Send(new GetMyWishlistQuery()));

        [HttpPost("{productId:guid}")]
        public async Task<IActionResult> Add(Guid productId)
        {
            await _mediator.Send(new AddToWishlistCommand(productId));
            return Ok("Added to wishlist successfully.");
        }

        [HttpDelete("{productId:guid}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _mediator.Send(new RemoveFromWishlistCommand(productId));
            return Ok("Removed from wishlist successfully.");
        }
    }
}
