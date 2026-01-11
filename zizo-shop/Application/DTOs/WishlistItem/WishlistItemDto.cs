namespace zizo_shop.Application.DTOs.WishlistItem
{
    public record WishlistItemDto(
        Guid Id,
        Guid ProductId,
        string ProductName,
        decimal ProductPrice,
        string CoverImageUrl
    );

}
