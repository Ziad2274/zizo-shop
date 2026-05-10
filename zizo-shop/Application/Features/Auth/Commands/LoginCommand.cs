using MediatR;
using zizo_shop.Application.DTOs.Auth;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;
}
