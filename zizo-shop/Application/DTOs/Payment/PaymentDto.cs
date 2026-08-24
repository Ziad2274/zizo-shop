namespace zizo_shop.Application.DTOs.Payment
{
    public record PaymentDto(
        Guid Id,
        Guid OrderId,
        decimal Amount,
        string Status,
        string Provider,
        string? PaymentIntentId,
        DateTime CreatedAt);
}