using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.API.Services;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Product;
using zizo_shop.Application.Features.Products.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Products.Handelers
{
    public class GetProductsQueryHandler
        : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService ;

        public GetProductsQueryHandler(IApplicationDbContext context, ICurrentUserService c )
        {
            _context = context;
            _currentUserService = c;
        }

        public async Task<List<ProductDto>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            var UserId = _currentUserService.UserId;
            return await _context.Products
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
                    // Efficiently check wishlist status in the SQL query
                    IsInWishlist = UserId != Guid.Empty && _context.WishlistItems
                        .Any(w => w.ProductId == p.Id && w.UserId == UserId)
                })
                .ToListAsync(cancellationToken);
        }
    }

}
