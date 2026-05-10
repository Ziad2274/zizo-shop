using MediatR;

namespace zizo_shop.Application.Features.Address.Commands
{
    public record DeleteAddressCommand(Guid AddressId) : IRequest;
}
