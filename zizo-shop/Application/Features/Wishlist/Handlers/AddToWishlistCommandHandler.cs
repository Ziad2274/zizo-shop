using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Wishlist.Commands;
using zizo_shop.Domain.Entities;
namespace zizo_shop.Application.Features.Wishlist.Handlers
{
    public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddToWishlistCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var exists = await _context.WishlistItems
                .AnyAsync(
                    w => w.UserId == userId &&
                    w.ProductId == request.ProductId,
                    cancellationToken);
            if (exists)
                {
                throw new InvalidOperationException("Product is already in the wishlist.");

            }
            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ProductId = request.ProductId,
            };
            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync(cancellationToken);


        }
    }
}
