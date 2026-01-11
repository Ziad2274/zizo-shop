using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zizo_shop.Domain.Entities;

namespace zizo_shop.Application.Features.Products
{
    public record CreateProductCommand(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        List<ProductImage> Images,
        Guid CategoryId
    ) : IRequest<Guid>;

}
