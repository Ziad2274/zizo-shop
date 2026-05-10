using MediatR;
namespace zizo_shop.Application.Features.Categories.Commands
{
    public record UpdateCategoryCommand(Guid Id, string Name, string Slug) : IRequest;
}
