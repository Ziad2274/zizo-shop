namespace zizo_shop.Application.DTOs.Cart
{
    public record CartDto(List<CartItemDto> Items, decimal Total);

}
