using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.WishlistItem;
using zizo_shop.Application.Features.Wishlist.Queries;

namespace zizo_shop.Application.Features.Wishlist.Handelers
{
    public class GetMyWishlistQueryHandler : IRequestHandler<GetMyWishlistQuery, List<WishlistItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetMyWishlistQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public Task<List<WishlistItemDto>> Handle(GetMyWishlistQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var wishlistItem = _context.WishlistItems
                 .Where(w => w.UserId == userId)
            .Select(w => new WishlistItemDto(
                w.Id,
                w.ProductId,
                w.Product.Name,
                w.Product.Price,
                w.Product.Images
                    .Where(i => i.IsCover)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()!
            ))
            .ToListAsync(cancellationToken);
            return wishlistItem;
        }
    }
}
