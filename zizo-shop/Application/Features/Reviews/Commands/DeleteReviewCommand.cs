using MediatR;
namespace zizo_shop.Application.Features.Reviews.Commands
{
    public record DeleteReviewCommand(Guid ReviewId) : IRequest;
}
