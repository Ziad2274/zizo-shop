using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Categories.Commands;
using zizo_shop.Domain.Entities;
namespace zizo_shop.Application.Features.Categories.Handlers
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateCategoryCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category { Name = request.Name, Slug = request.Slug, ParentCategoryId = request.ParentCategoryId };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
