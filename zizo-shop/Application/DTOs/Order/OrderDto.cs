namespace zizo_shop.Application.DTOs.Order
{
    public record OrderDto(
        Guid Id,
        DateTime CreatedAt,
        decimal TotalPrice,
        decimal SubTotal,
        decimal ShippingFee,
        string Status,
        int ItemCount
    );
}
