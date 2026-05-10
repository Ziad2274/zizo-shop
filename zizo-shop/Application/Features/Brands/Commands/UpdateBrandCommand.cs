using MediatR;
namespace zizo_shop.Application.Features.Brands.Commands
{
    public record UpdateBrandCommand(Guid Id, string Name, string Slug, bool IsActive) : IRequest;
}
