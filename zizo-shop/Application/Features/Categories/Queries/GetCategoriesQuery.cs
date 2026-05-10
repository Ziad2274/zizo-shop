using MediatR;
using zizo_shop.Application.DTOs.Category;
namespace zizo_shop.Application.Features.Categories.Queries
{
    public record GetCategoriesQuery() : IRequest<List<CategoryDto>>;
}
