using MediatR;
using MediatR;

namespace zizo_shop.Application.Features.Wishlist.Commands
{

    public record AddToWishlistCommand(Guid ProductId) : IRequest;

}
