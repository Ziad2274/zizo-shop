using MediatR;
namespace zizo_shop.Application.Features.Products.Commands
{
    public record SetProductDiscountCommand(Guid ProductId, decimal? DiscountPrice) : IRequest;
}
