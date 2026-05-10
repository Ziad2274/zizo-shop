using MediatR;
namespace zizo_shop.Application.Features.Categories.Commands
{
    public record CreateCategoryCommand(string Name, string Slug, Guid? ParentCategoryId) : IRequest<Guid>;
}
