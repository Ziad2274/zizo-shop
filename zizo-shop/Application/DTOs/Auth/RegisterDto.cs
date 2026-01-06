namespace zizo_shop.Application.DTOs.Auth
{
    public record RegisterDto(
      string FirstName,
      string LastName,
      string Email,
      string Phone,
      string Password
  );

}
