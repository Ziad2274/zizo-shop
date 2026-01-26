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

       public AddToCartCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService
            )
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task  Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
            if (product == null)
                throw new Exception("Product not found.");
            var cart = await _context.Carts.Include(c=>c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
            if (cart == null)
            {
                cart = new Domain.Entities.Cart
                (
                     userId
               );           
                _context.Carts.Add(cart);
            }
            cart.AddItem(product, request.Quantity);
            await _context.SaveChangesAsync(cancellationToken);


        }
    }
}
