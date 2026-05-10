using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Address.Commands;

namespace zizo_shop.Application.Features.Address.Handlers
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateAddressCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var address = new Domain.Entities.Address
            {
                UserId = _currentUserService.UserId,
                City = request.City,
                Street = request.Street,
                ZipCode = request.ZipCode
            };
            _context.Address.Add(address);
            await _context.SaveChangesAsync(cancellationToken);
            return address.Id;
        }
    }
}
