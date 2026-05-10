using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Categories.Commands;
using zizo_shop.Application.Features.Categories.Queries;

namespace zizo_shop.API.Controllers.Categories
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetCategoriesQuery()));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetAll), new { id }, new { Id = id });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand cmd)
        {
            await _mediator.Send(cmd with { Id = id });
            return Ok("Category updated.");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return Ok("Category deleted.");
        }
    }
}
