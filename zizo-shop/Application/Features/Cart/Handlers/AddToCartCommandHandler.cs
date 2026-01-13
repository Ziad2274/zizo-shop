using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.Features.Cart.Commands;
namespace zizo_shop.Application.Features.Cart.Handelers
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddToCartCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task  Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var cart=await _context.Carts
                .Include(c=>c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId,cancellationToken);
            if(cart==null)
            {
                cart=new Domain.Entities.Cart 
                {
                    UserId=userId,
                    Items=new List<Domain.Entities.CartItem>()
                };
                _context.Carts.Add(cart);
            }
            var cartItem=cart.Items
                .FirstOrDefault(pi => pi.ProductId == request.ProductId );
            if(cartItem!=null)
                cartItem.Quantity+=request.Quantity;
            else
            { 
                cart.Items.Add(
                    new Domain.Entities.CartItem
                    {
                        ProductId=request.ProductId,
                        Quantity=request.Quantity
                    }

                    );
            }
            await _context.SaveChangesAsync(cancellationToken);


        }
    }
}
