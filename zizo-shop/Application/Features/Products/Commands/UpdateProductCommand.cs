using MediatR;
namespace zizo_shop.Application.Features.Products.Commands
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        Guid CategoryId,
        bool IsActive
    ) : IRequest;
}
