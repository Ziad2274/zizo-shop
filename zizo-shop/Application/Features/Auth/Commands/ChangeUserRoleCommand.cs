using MediatR;

namespace zizo_shop.Application.Features.Auth.Commands
{
    public record ChangeUserRoleCommand(Guid UserId, string NewRole) : IRequest;

    

    }
