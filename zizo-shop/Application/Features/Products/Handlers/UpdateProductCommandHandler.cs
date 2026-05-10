using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateProductCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");

            // Use the existing domain method where possible; update settable props directly
            product.IsActive = request.IsActive;
            product.SKU = product.SKU; // SKU unchanged
            // Reflection-free: expose internal setters via a domain method
            product.UpdateDetails(request.Name, request.Description, request.Price, request.Stock, request.CategoryId);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
