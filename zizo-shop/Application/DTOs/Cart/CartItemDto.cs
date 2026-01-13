namespace zizo_shop.Application.DTOs.Cart
{
    public record CartItemDto(
            Guid Id,
            Guid ProductId,
            string ProductName,
            string? ImageCover,
            decimal Price,
            int Quantity,
            decimal SubTotal 
        );
}
