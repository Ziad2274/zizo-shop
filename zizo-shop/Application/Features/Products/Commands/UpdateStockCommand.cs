using MediatR;

namespace zizo_shop.Application.Features.Products.Commands
{
    public record UpdateStockCommand(Guid ProductId, int NewStockQuantity) : IRequest;
}
