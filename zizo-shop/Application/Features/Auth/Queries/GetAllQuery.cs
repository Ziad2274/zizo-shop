namespace zizo_shop.Application.Features.Auth.Queries
{
    using MediatR;

    public record GetAllUsersQuery() : IRequest<List<string>>;

}
