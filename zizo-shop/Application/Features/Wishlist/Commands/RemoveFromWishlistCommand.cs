using MediatR;

namespace zizo_shop.Application.Features.Wishlist.Commands
{

    public record RemoveFromWishlistCommand(Guid ProductId) : IRequest;

}
