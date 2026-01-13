using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Common.Interfaces;

namespace zizo_shop.API.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly IFileService _fileService;
        public ProductImageController( IApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage(Guid productId,[FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            
            var imageUrl = await _fileService.UploadFileAsync(file,"products") ;
            _context.ProductImages.Add(new Domain.Entities.ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl
            });
           await _context.SaveChangesAsync();
            return Ok(new { ImageUrl = imageUrl });
        }

    }
}