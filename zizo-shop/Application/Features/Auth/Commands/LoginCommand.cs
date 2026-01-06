using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;

}
