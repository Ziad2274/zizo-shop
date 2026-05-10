namespace zizo_shop.Application.DTOs.Order
{
    public record OrderItemDto(Guid ProductId, string ProductName, decimal Price, int Quantity, decimal SubTotal);
    public record OrderDetailDto(
        Guid Id, DateTime CreatedAt, decimal TotalPrice, decimal SubTotal,
        decimal ShippingFee, string Status, string UserEmail,
        Guid AddressId, List<OrderItemDto> Items);
}
