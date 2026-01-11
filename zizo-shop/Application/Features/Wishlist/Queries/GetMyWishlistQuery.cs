using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Application.DTOs;
using zizo_shop.Application.DTOs.WishlistItem;

namespace zizo_shop.Application.Features.Wishlist.Queries
{
    public record GetMyWishlistQuery()
        : IRequest<List<WishlistItemDto>>;
}
