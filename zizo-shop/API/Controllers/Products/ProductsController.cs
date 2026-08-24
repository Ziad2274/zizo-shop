using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Products;
using zizo_shop.Application.Features.Products.Commands;
using zizo_shop.Application.Features.Products.Queries;

namespace zizo_shop.API.Controllers.Products
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] Guid? categoryId,
            [FromQuery] Guid? brandId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? inStock,
            [FromQuery] string? sortBy,
            [FromQuery] int? pageNumber = 1,
            [FromQuery] int? pageSize = 10


            ) => Ok(await _mediator.Send(new GetProductsQuery(
                search, pageNumber, pageSize, categoryId, brandId, minPrice, maxPrice, inStock, sortBy
                )));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetProductByIdQuery(id)));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand cmd)
        {
            await _mediator.Send(cmd with { Id = id });
            return Ok("Product updated.");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return Ok("Product deleted.");
        }

        [HttpPatch("{id:guid}/discount")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetDiscount(Guid id, [FromBody] SetProductDiscountCommand cmd)
        {
            await _mediator.Send(cmd with { ProductId = id });
            return Ok("Discount updated.");
        }
    }
}
