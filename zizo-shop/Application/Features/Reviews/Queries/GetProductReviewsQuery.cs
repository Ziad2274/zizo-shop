using MediatR;
using zizo_shop.Application.DTOs.Review;
namespace zizo_shop.Application.Features.Reviews.Queries
{
    public record GetProductReviewsQuery(Guid ProductId) : IRequest<List<ReviewDto>>;
}
