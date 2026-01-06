using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,
        string Phone,
        string Password
    ) : IRequest;


}
