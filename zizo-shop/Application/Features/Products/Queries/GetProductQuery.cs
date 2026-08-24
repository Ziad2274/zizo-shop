using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Application.DTOs.Product;

namespace zizo_shop.Application.Features.Products.Queries
{
    public record GetProductsQuery(
        string?Search = null,
        int? PageNumber = 1,
        int? PageSize = 10,
        Guid?CategoryId = null,
        Guid?BrandId = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        bool? InStock = null,
        string? SortBy = null




        ) : IRequest<PagedResult<ProductDto>>;
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize,
        int TotalPages
    );
}
