using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Address.Commands;

namespace zizo_shop.Application.Features.Address.Handlers
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteAddressCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _context.Address
                .FirstOrDefaultAsync(a => a.Id == request.AddressId && a.UserId == _currentUserService.UserId, cancellationToken);

            if (address == null)
                throw new KeyNotFoundException("Address not found.");

            _context.Address.Remove(address);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
