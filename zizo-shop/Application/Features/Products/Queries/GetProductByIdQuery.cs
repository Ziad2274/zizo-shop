using MediatR;
using zizo_shop.Application.DTOs.Product;

namespace zizo_shop.Application.Features.Products.Queries
{
    public record GetProductByIdQuery(Guid ProductId) : IRequest<ProductDto>;
}
