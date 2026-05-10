using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Brand;
using zizo_shop.Application.Features.Brands.Queries;
namespace zizo_shop.Application.Features.Brands.Handlers
{
    public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, List<BrandDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetBrandsQueryHandler(IApplicationDbContext context) => _context = context;
        public async Task<List<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
            => await _context.Brands
                .Select(b => new BrandDto(b.Id, b.Name, b.Slug, b.IsActive))
                .ToListAsync(cancellationToken);
    }
}
