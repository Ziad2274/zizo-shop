using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using zizo_shop.Application.Features.Wishlist.Commands;
using zizo_shop.Application.Features.Wishlist.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace zizo_shop.API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishlistController : ControllerBase
    {

        IMediator _mediator;
        public WishlistController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet()]
        public async Task<IActionResult> GetMyWishlist()
        {
            return Ok(await _mediator.Send(new GetMyWishlistQuery()));
        }

        // POST api/<WishlistController>
        [HttpPost]
        public async Task<IActionResult> Add(Guid productId)
        {
            await _mediator.Send(new AddToWishlistCommand(productId));
            return Ok("Added Successfully");
        }

    
        // DELETE api/<WishlistController>/
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Remove(Guid productId)
        {
            await _mediator.Send(new RemoveFromWishlistCommand(productId));
            return Ok("Removed Successfully");
        }
    }
}
