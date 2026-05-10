using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Product;
using zizo_shop.Application.Features.Products.Queries;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetProductsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var wishlistProductIds = userId != Guid.Empty
                ? await _context.WishlistItems
                    .Where(w => w.UserId == userId)
                    .Select(w => w.ProductId)
                    .ToHashSetAsync(cancellationToken)
                : new HashSet<Guid>();

            return await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    Stock = p.StockQuantity,
                    SKU = p.SKU,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    IsInWishlist = wishlistProductIds.Contains(p.Id),
                    ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = p.Reviews.Count
                })
                .ToListAsync(cancellationToken);
        }
    }
}
