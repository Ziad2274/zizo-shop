using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Queries;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class GetCartTotalPriceQueryHandler : IRequestHandler<GetCartTotalPriceQuery, decimal>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetCartTotalPriceQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<decimal> Handle(GetCartTotalPriceQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            return await _context.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .SumAsync(ci => (ci.Product.DiscountPrice ?? ci.Product.Price) * ci.Quantity, cancellationToken);
        }
    }
}
