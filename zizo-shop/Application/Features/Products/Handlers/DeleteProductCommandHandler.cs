using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteProductCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");
            product.MarkAsDeleted();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
