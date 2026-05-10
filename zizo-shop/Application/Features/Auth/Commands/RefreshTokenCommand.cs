using MediatR;
using zizo_shop.Application.DTOs.Auth;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record RefreshTokenCommand(string Token) : IRequest<AuthResponseDto>;
}
