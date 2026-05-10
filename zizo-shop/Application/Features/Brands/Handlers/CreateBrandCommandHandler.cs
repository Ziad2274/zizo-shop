using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Brands.Commands;
using zizo_shop.Domain.Entities;
namespace zizo_shop.Application.Features.Brands.Handlers
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateBrandCommandHandler(IApplicationDbContext context) => _context = context;
        public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = new Brand { Name = request.Name, Slug = request.Slug };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync(cancellationToken);
            return brand.Id;
        }
    }
}
