using MediatR;
using zizo_shop.Application.DTOs.Address;

namespace zizo_shop.Application.Features.Address.Queries
{
    public record GetMyAddressesQuery() : IRequest<List<AddressDto>>;
}
