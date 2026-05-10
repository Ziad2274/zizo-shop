using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Brands.Commands;
namespace zizo_shop.Application.Features.Brands.Handlers
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteBrandCommandHandler(IApplicationDbContext context) => _context = context;
        public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _context.Brands.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Brand not found.");
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
