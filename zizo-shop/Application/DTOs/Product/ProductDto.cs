namespace zizo_shop.Application.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public string? SKU { get; set; }
        public bool IsActive { get; set; }
        public bool IsInWishlist { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
