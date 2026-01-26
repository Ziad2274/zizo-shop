using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommand, string>
    {
        private readonly IFileService _fileService;
        private readonly IApplicationDbContext _context;
        public UploadProductImageCommandHandler(IFileService fileService, IApplicationDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }
        public async Task<string> Handle(UploadProductImageCommand request, CancellationToken cancellationToken)
        {
            var productExist = await _context.Products
                .AnyAsync(p=>p.Id==request.ProductId,cancellationToken);
            if (!productExist )
            {
                throw new Exception("Product not found");
            }
            var imagePath = await _fileService.UploadFileAsync(request.File, "product-images");
            var productImage = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = imagePath
            };
            _context.ProductImages.Add(productImage);
          await  _context.SaveChangesAsync(cancellationToken);
            return imagePath;

        }
    }
}
