using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;
using zizo_shop.Infrastructure.Services;

namespace zizo_shop.API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
   
       private readonly IMediator _mediator;
        public ProductImageController(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage([FromForm] Guid productId,[FromForm] IFormFile file)
        {
            var imageUrl = await _mediator.Send(new UploadProductImageCommand{ ProductId = productId, File = file });

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
           
            return Ok(new { ImageUrl = imageUrl });
        }

    }
}