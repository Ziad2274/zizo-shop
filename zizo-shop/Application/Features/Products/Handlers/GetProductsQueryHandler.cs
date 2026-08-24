using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Product;
using zizo_shop.Application.Features.Products.Queries;

namespace zizo_shop.Application.Features.Products.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto> >
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetProductsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var wishlistProductIds = userId != Guid.Empty
                ? await _context.WishlistItems
                    .Where(w => w.UserId == userId)
                    .Select(w => w.ProductId)
                    .ToHashSetAsync(cancellationToken)
                : new HashSet<Guid>();

            var query =  _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .AsQueryable();
            //filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term= request.Search.ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    p.Description.ToLower().Contains(term) ||
                    p.Category.Name.ToLower().Contains(term));
            }
            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId);
            if (request.BrandId.HasValue)
                query = query.Where(p => p.BrandId == request.BrandId);
            if (request.MinPrice.HasValue)
                query = query.Where(p =>( p.DiscountPrice??p.Price)>=request.MinPrice);
            if (request.MaxPrice.HasValue)
                query = query.Where(p => (p.DiscountPrice ?? p.Price) >= request.MaxPrice);
            if (request.InStock.HasValue)
                query = query.Where(p => request.InStock.Value ? p.StockQuantity > 0 : p.StockQuantity == 0);
            if (request.InStock.HasValue)
                query = query.Where(p => request.InStock.Value ? p.StockQuantity > 0 : p.StockQuantity == 0);
            //sorting
            query = request.SortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.DiscountPrice ?? p.Price),
                "price_desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "rating_desc" => query.OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0),
                _ => query.OrderBy(p => p.Name)
            
            };
            //pagination
            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize.Value);
            var pageSize =Math.Clamp((int) request.PageSize,1,50);
            var page = Math.Max((int)request.PageNumber, 1);
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
            return new PagedResult<ProductDto>(products, totalCount, page, pageSize, totalPages);
        }
    }
}
