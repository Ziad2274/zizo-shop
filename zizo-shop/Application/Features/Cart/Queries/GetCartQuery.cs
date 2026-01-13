using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Application.DTOs;
using zizo_shop.Application.DTOs.Cart;

namespace zizo_shop.Application.Features.Cart.Queries
{
   
    public record GetCartQuery : IRequest<CartDto?>;
}
