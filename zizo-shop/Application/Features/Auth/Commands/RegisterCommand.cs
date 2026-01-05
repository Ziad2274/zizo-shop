using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record RegisterCommand
    (
        string Email,
        string Password

    ) : IRequest<string>;
    
}
