using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Queries;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class GetCartItemCountQueryHandler : IRequestHandler<GetCartItemCountQuery, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetCartItemCountQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.CartItems
                .Where(c => c.Cart.UserId == userId)
                .SumAsync(x => x.Quantity, cancellationToken)
                ;

        }
    }
}