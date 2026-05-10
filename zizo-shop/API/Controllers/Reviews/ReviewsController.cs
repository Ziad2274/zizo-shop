using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Reviews.Commands;
using zizo_shop.Application.Features.Reviews.Queries;

namespace zizo_shop.API.Controllers.Reviews
{
    [ApiController]
    [Route("api/products/{productId:guid}/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetReviews(Guid productId)
            => Ok(await _mediator.Send(new GetProductReviewsQuery(productId)));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Guid productId, [FromBody] CreateReviewCommand cmd)
        {
            var id = await _mediator.Send(cmd with { ProductId = productId });
            return CreatedAtAction(nameof(GetReviews), new { productId }, new { Id = id });
        }

        [HttpDelete("{reviewId:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid reviewId)
        {
            await _mediator.Send(new DeleteReviewCommand(reviewId));
            return Ok("Review deleted.");
        }
    }
}
