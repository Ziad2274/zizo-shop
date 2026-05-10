using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Brands.Commands;
namespace zizo_shop.Application.Features.Brands.Handlers
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateBrandCommandHandler(IApplicationDbContext context) => _context = context;
        public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _context.Brands.FindAsync(new object[] { request.Id }, cancellationToken)
                ?? throw new KeyNotFoundException("Brand not found.");
            brand.Name = request.Name;
            brand.Slug = request.Slug;
            brand.IsActive = request.IsActive;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
