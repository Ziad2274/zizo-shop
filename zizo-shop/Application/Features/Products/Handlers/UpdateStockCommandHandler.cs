using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateStockCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateStockCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken) ?? throw new KeyNotFoundException("Product not found.");
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");

            }
            product.UpdateStock(request.NewStockQuantity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
