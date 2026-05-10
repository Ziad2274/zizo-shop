using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Products.Commands;

namespace zizo_shop.API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] Guid productId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var imageUrl = await _mediator.Send(new UploadProductImageCommand { ProductId = productId, File = file });
            return Ok(new { ImageUrl = imageUrl });
        }
    }
}
