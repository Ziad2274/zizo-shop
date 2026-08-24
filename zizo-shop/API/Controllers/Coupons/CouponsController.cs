using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Coupons.Commands;
using zizo_shop.Application.Features.Coupons.Queries;

namespace zizo_shop.API.Controllers.Coupons
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _mediator.Send(new GetCouponsQuery()));
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCouponCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetAll), new { id }, new { Id = id });

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCouponCommand(id));
            return Ok("Coupon deleted.");
        }
        [HttpPost("apply")]
        [Authorize]
        public async Task<IActionResult> Apply([FromBody] ApplyCouponCommand cmd)
            => Ok(await _mediator.Send(cmd));

    }
}

