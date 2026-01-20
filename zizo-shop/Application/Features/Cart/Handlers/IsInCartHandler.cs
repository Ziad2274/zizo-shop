using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Queries;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class IsInCartHandler : IRequestHandler<IsInCartQuery, bool>
    {
        private readonly ICurrentUserService _currentUserService;   
        private readonly IApplicationDbContext _context;
        public async Task<bool> Handle(IsInCartQuery request, CancellationToken cancellationToken)
        {
            return await _context.CartItems
                .AnyAsync(x => x.ProductId == request.ProductId
                             && x.Cart.UserId == _currentUserService.UserId, cancellationToken);
        }
    }
}
