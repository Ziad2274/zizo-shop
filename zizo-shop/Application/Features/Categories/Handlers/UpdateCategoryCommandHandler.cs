using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Categories.Commands;
namespace zizo_shop.Application.Features.Categories.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateCategoryCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var cat = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Category not found.");
            cat.Name = request.Name;
            cat.Slug = request.Slug;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
