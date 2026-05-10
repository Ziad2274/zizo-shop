using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Categories.Commands;
namespace zizo_shop.Application.Features.Categories.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteCategoryCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var cat = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Category not found.");
            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
