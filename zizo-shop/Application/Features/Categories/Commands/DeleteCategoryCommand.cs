using MediatR;
namespace zizo_shop.Application.Features.Categories.Commands
{
    public record DeleteCategoryCommand(Guid Id) : IRequest;
}
