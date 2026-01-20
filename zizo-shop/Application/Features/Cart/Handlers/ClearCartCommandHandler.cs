using MediatR;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Commands;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public ClearCartCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;

        }

        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var cart = _context.Carts
                .FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
                {
                var cartItems = _context.CartItems
                    .Where(ci => ci.CartId == cart.Id);
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
