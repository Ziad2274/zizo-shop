using MediatR;
namespace zizo_shop.Application.Features.Reviews.Commands
{
    public record CreateReviewCommand(Guid ProductId, int Rating, string Comment) : IRequest<Guid>;
}
