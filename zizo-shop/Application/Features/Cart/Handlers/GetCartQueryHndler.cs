using MediatR;
using Microsoft.EntityFrameworkCore;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs.Cart;
using zizo_shop.Application.Features.Cart.Queries;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Cart.Handelers
{
    public class GetCartQueryHndler : IRequestHandler<GetCartQuery, CartDto?>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _context;
        public async Task<CartDto?> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var cart = await _context.Carts
                .Include(i => i.Items)
                .ThenInclude(p => p.Product).ThenInclude(pi => pi.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId,cancellationToken);

            if (cart == null) {
                return null;
            }
            var itemDtos = cart.Items.Select(i => new CartItemDto(
                        i.Id,                                     // Guid Id
                        i.ProductId,                              // Guid ProductId
                        i.Product.Name,                           // string ProductName
                        i.Product.Images.FirstOrDefault()?.ImageUrl, // string? ImageCover
                        i.Product.DiscountPrice ?? i.Product.Price, // decimal Price
                        i.Quantity,                               // int Quantity
                        (i.Product.DiscountPrice ?? i.Product.Price) * i.Quantity // decimal SubTotal
                    )).ToList();

                    //.Select(c => new CartDto
            //{
            //    Id = c.Id,
            //    UserId = c.UserId,
            //    Items = c.Items.Select(i => new CartItemDto
            //    {
            //        Id = i.Id,
            //        ProductId = i.ProductId,
            //        Quantity = i.Quantity
            //    }).ToList()
            //})
            //.FirstOrDefault();
            return new CartDto( itemDtos,itemDtos.Sum(s=>s.SubTotal));

        }
    }
}
