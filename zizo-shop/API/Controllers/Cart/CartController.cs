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
            return Ok("Added Successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            return Ok(await _mediator.Send(new GetCartQuery()));
        }
    }
}
