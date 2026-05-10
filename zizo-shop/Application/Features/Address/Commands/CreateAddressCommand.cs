using MediatR;

namespace zizo_shop.Application.Features.Address.Commands
{
    public record CreateAddressCommand(string City, string Street, string ZipCode) : IRequest<Guid>;
}
