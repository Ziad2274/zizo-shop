using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Commands;

namespace zizo_shop.Application.Features.Cart.Handlers
{
    public class UpdateCartQuaantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCartQuaantityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        // Change return type from Task to Task<Unit>
        public async Task<Unit> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.Cart.UserId == userId, cancellationToken);

            if (cartItem != null)
            {
                cartItem.Quantity = request.Quantity;
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            // You must return Unit.Value
            return Unit.Value;
        }
    }
}