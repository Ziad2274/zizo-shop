using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Wishlist.Commands;

namespace zizo_shop.Application.Features.Wishlist.Handlers
{
    public class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RemoveFromWishlistCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == request.ProductId, cancellationToken);

            if (item == null)
                throw new KeyNotFoundException("Product not found in wishlist.");

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
