namespace zizo_shop.Application.DTOs.Auth
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string? Phone,
        IList<string> Roles
    );
}
