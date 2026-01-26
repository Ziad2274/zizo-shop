using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Handellers.Products
{
    // Note: [Authorize] should be moved to the Controller, not the Handler.
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
      

            var product = new Product(
                request.Name,
                request.Description,
                request.Price,
                request.Stock,
                request.CategoryId
            );

            foreach (var img in request.Images)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = img.ImageUrl,
                    IsCover = img.IsCover
                });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}