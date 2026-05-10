using MediatR;
using zizo_shop.Application.DTOs.Brand;
namespace zizo_shop.Application.Features.Brands.Queries
{
    public record GetBrandsQuery() : IRequest<List<BrandDto>>;
}
