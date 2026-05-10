using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Products.Commands;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class SetProductDiscountCommandHandler : IRequestHandler<SetProductDiscountCommand>
    {
        private readonly IApplicationDbContext _context;
        public SetProductDiscountCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(SetProductDiscountCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken)
                ?? throw new KeyNotFoundException("Product not found.");

            if (request.DiscountPrice.HasValue)
                product.AddDiscount(request.DiscountPrice.Value);
            else
                product.RemoveDiscount();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
