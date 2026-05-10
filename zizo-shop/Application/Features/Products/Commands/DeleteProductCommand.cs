using MediatR;
namespace zizo_shop.Application.Features.Products.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest;
}
