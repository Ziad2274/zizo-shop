using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Address;
using zizo_shop.Application.Features.Address.Queries;

namespace zizo_shop.Application.Features.Address.Handlers
{
    public class GetMyAddressesQueryHandler : IRequestHandler<GetMyAddressesQuery, List<AddressDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMyAddressesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<AddressDto>> Handle(GetMyAddressesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.Address
                .Where(a => a.UserId == userId)
                .Select(a => new AddressDto(a.Id, a.City, a.Street, a.ZipCode))
                .ToListAsync(cancellationToken);
        }
    }
}
