using MediatR;
using zizo_shop.Application.DTOs.Auth;

namespace zizo_shop.Application.Features.Auth.Queries
{
    public record GetAllUsersQuery() : IRequest<List<UserDto>>;
}
