using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Application.Common.Interfaces;
using zizo_shop.Application.DTOs;
using zizo_shop.Application.Features.Products.Queries;
using zizo_shop.Infrastructure.Identity;

namespace zizo_shop.Application.Features.Products.Handelers
{
    public class GetProductsQueryHandler
        : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(
            GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price
                })
                .ToListAsync(cancellationToken);
        }
    }

}
