using MediatR;
namespace zizo_shop.Application.Features.Brands.Commands
{
    public record DeleteBrandCommand(Guid Id) : IRequest;
}
