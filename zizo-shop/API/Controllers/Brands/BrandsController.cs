using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Brands.Commands;
using zizo_shop.Application.Features.Brands.Queries;

namespace zizo_shop.API.Controllers.Brands
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BrandsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetBrandsQuery()));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateBrandCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetAll), new { id }, new { Id = id });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBrandCommand cmd)
        {
            await _mediator.Send(cmd with { Id = id });
            return Ok("Brand updated.");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteBrandCommand(id));
            return Ok("Brand deleted.");
        }
    }
}
