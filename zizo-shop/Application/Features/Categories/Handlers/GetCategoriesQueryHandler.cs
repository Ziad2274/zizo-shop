using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Category;
using zizo_shop.Application.Features.Categories.Queries;
namespace zizo_shop.Application.Features.Categories.Handlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetCategoriesQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var all = await _context.Categories.Include(c => c.SubCategories).ToListAsync(cancellationToken);
            return all.Where(c => c.ParentCategoryId == null)
                      .Select(c => Map(c)).ToList();
        }

        private static CategoryDto Map(Domain.Entities.Category c) =>
            new(c.Id, c.Name, c.Slug, c.ParentCategoryId,
                c.SubCategories.Select(s => Map(s)).ToList());
    }
}
