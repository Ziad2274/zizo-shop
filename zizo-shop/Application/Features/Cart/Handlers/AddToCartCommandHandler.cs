using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Commands;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddToCartCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService
        )
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
                throw new Exception("Product not found.");

            // Use full namespace to avoid any 'Cart' namespace vs class collision
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                cart = new zizo_shop.Domain.Entities.Cart(userId);
                _context.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.MarkAsUpdate();
            }
            else
            {
                var newItem = new zizo_shop.Domain.Entities.CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = request.Quantity
                };

                // Explicitly add to the DbSet to guarantee an INSERT command is generated
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}