using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Product;
using zizo_shop.Application.Features.Products.Queries;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetProductByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var wishlistIds = userId != Guid.Empty
                ? await _context.WishlistItems
                    .Where(w => w.UserId == userId)
                    .Select(w => w.ProductId)
                    .ToHashSetAsync(cancellationToken)
                : new HashSet<Guid>();

            var product = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Where(p => p.Id == request.ProductId)
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
                    IsInWishlist = wishlistIds.Contains(p.Id),
                    ImageUrls = p.Images.Select(i => i.ImageUrl).ToList(),
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = p.Reviews.Count
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            return product;
        }
    }
}
