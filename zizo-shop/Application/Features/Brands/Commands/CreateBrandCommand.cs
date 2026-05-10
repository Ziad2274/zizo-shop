using MediatR;
namespace zizo_shop.Application.Features.Brands.Commands
{
    public record CreateBrandCommand(string Name, string Slug) : IRequest<Guid>;
}
