using MediatR;

namespace zizo_shop.Application.Features.Cart.Queries
{
    public record IsInCartQuery : IRequest<bool> { public Guid ProductId { get; set; } }
}
